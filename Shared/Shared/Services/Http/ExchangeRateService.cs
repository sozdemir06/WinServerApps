using System.Globalization;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;

namespace Shared.Services.Http
{
  public class ExchangeRateService : IExchangeRateService
  {
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly IDatabase _redis;

    public ExchangeRateService(
        ILogger<ExchangeRateService> logger,
        IConfiguration configuration,
        IDatabase redis)
    {
      _logger = logger;
      _redis = redis;

      // TCMB API base address
      var baseAddress = configuration.GetSection("TCMB:BaseAddress").Value ?? "https://www.tcmb.gov.tr";

      _httpClient = new HttpClient
      {
        BaseAddress = new Uri(baseAddress)
      };

      _httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");
      _httpClient.DefaultRequestHeaders.Add("User-Agent", "WinApps/1.0");

      var timeoutSeconds = configuration.GetValue<int>("TCMB:TimeoutSeconds", 30);
      _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    }

    public async Task<ExchangeRateResponseDto> GetExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
      try
      {
        var cacheKey = "ExchangeRates:TCMB";
        var cachedData = await _redis.StringGetAsync(cacheKey);

        if (cachedData.HasValue)
        {
          var cachedRates = System.Text.Json.JsonSerializer.Deserialize<ExchangeRateResponseDto>(cachedData.ToString());
          if (cachedRates != null && cachedRates.LastUpdateTime.Date == DateTime.Today)
          {
            _logger.LogInformation("Returning cached exchange rates from {LastUpdateTime}", cachedRates.LastUpdateTime);
            return cachedRates;
          }
        }

        _logger.LogInformation("Fetching exchange rates from TCMB API");

        // TCMB XML API endpoint
        var response = await _httpClient.GetStringAsync("/kurlar/today.xml", cancellationToken);

        if (string.IsNullOrEmpty(response))
        {
          throw new InvalidOperationException("Empty response from TCMB API");
        }

        var exchangeRates = ParseTCMBXmlResponse(response);
        var result = new ExchangeRateResponseDto
        {
          ExchangeRates = exchangeRates,
          LastUpdateTime = DateTime.Now,
          Source = "TCMB"
        };

        // Cache for 1 hour
        var cacheExpiry = TimeSpan.FromHours(1);
        await _redis.StringSetAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(result), cacheExpiry);

        _logger.LogInformation("Successfully fetched {Count} exchange rates from TCMB", exchangeRates.Count);
        return result;
      }
      catch (HttpRequestException ex)
      {
        _logger.LogError(ex, "HTTP request failed when fetching exchange rates from TCMB");
        throw;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unexpected error occurred when fetching exchange rates from TCMB");
        throw;
      }
    }

    public async Task<ExchangeRateDto?> GetExchangeRateByCurrencyAsync(string currencyCode, CancellationToken cancellationToken = default)
    {
      try
      {
        var allRates = await GetExchangeRatesAsync(cancellationToken);
        return allRates.ExchangeRates.FirstOrDefault(x =>
            x.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error getting exchange rate for currency: {CurrencyCode}", currencyCode);
        throw;
      }
    }

    public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken = default)
    {
      try
      {
        if (fromCurrency.Equals("TRY", StringComparison.OrdinalIgnoreCase))
        {
          // TL'den başka bir para birimine çevirme
          var targetRate = await GetExchangeRateByCurrencyAsync(toCurrency, cancellationToken);
          if (targetRate == null)
            throw new InvalidOperationException($"Exchange rate not found for currency: {toCurrency}");

          return amount / targetRate.ForexSelling;
        }
        else if (toCurrency.Equals("TRY", StringComparison.OrdinalIgnoreCase))
        {
          // Başka bir para biriminden TL'ye çevirme
          var sourceRate = await GetExchangeRateByCurrencyAsync(fromCurrency, cancellationToken);
          if (sourceRate == null)
            throw new InvalidOperationException($"Exchange rate not found for currency: {fromCurrency}");

          return amount * sourceRate.ForexBuying;
        }
        else
        {
          // İki farklı para birimi arasında çevirme (TL üzerinden)
          var fromRate = await GetExchangeRateByCurrencyAsync(fromCurrency, cancellationToken);
          var toRate = await GetExchangeRateByCurrencyAsync(toCurrency, cancellationToken);

          if (fromRate == null)
            throw new InvalidOperationException($"Exchange rate not found for currency: {fromCurrency}");
          if (toRate == null)
            throw new InvalidOperationException($"Exchange rate not found for currency: {toCurrency}");

          // Önce TL'ye çevir, sonra hedef para birimine çevir
          var tryAmount = amount * fromRate.ForexBuying;
          return tryAmount / toRate.ForexSelling;
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error converting currency from {FromCurrency} to {ToCurrency} for amount {Amount}",
            fromCurrency, toCurrency, amount);
        throw;
      }
    }

    private List<ExchangeRateDto> ParseTCMBXmlResponse(string xmlResponse)
    {
      try
      {
        var doc = XDocument.Parse(xmlResponse);
        var rates = new List<ExchangeRateDto>();

        // TCMB XML yapısını parse et
        var currencyElements = doc.Descendants("Currency");

        foreach (var currency in currencyElements)
        {
          var currencyCode = currency.Attribute("Kod")?.Value;
          if (string.IsNullOrEmpty(currencyCode))
            continue;

          var forexBuying = currency.Element("ForexBuying")?.Value;
          var forexSelling = currency.Element("ForexSelling")?.Value;
          var banknoteBuying = currency.Element("BanknoteBuying")?.Value;
          var banknoteSelling = currency.Element("BanknoteSelling")?.Value;

          if (decimal.TryParse(forexBuying, NumberStyles.Any, CultureInfo.InvariantCulture, out var fb) &&
              decimal.TryParse(forexSelling, NumberStyles.Any, CultureInfo.InvariantCulture, out var fs) &&
              decimal.TryParse(banknoteBuying, NumberStyles.Any, CultureInfo.InvariantCulture, out var bb) &&
              decimal.TryParse(banknoteSelling, NumberStyles.Any, CultureInfo.InvariantCulture, out var bs))
          {
            rates.Add(new ExchangeRateDto
            {
              CurrencyCode = currencyCode,
              CurrencyName = GetCurrencyName(currencyCode),
              ForexBuying = fb,
              ForexSelling = fs,
              BanknoteBuying = bb,
              BanknoteSelling = bs,
              Date = DateTime.Today
            });
          }
        }

        return rates;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error parsing TCMB XML response");
        throw new InvalidOperationException("Failed to parse TCMB XML response", ex);
      }
    }

    private string GetCurrencyName(string currencyCode)
    {
      return currencyCode switch
      {
        "USD" => "US DOLLAR",
        "EUR" => "EURO",
        "GBP" => "POUND STERLING",
        "JPY" => "JAPANESE YEN",
        "CHF" => "SWISS FRANC",
        "CAD" => "CANADIAN DOLLAR",
        "AUD" => "AUSTRALIAN DOLLAR",
        "CNY" => "CHINESE RENMINBI",
        "RUB" => "RUSSIAN ROUBLE",
        "KRW" => "SOUTH KOREAN WON",
        "SEK" => "SWEDISH KRONA",
        "NOK" => "NORWEGIAN KRONE",
        "DKK" => "DANISH KRONE",
        "SAR" => "SAUDI RIYAL",
        "BGN" => "BULGARIAN LEV",
        "RON" => "ROMANIAN LEU",
        "PLN" => "POLISH ZLOTY",
        "CZK" => "CZECH KORUNA",
        "HUF" => "HUNGARIAN FORINT",
        "HRK" => "CROATIAN KUNA",
        "BRL" => "BRAZILIAN REAL",
        "MXN" => "MEXICAN PESO",
        "INR" => "INDIAN RUPEE",
        "ZAR" => "SOUTH AFRICAN RAND",
        "THB" => "THAI BAHT",
        "SGD" => "SINGAPORE DOLLAR",
        "HKD" => "HONG KONG DOLLAR",
        "NZD" => "NEW ZEALAND DOLLAR",
        "MYR" => "MALAYSIAN RINGGIT",
        "IDR" => "INDONESIAN RUPIAH",
        "PHP" => "PHILIPPINE PESO",
        "TWD" => "NEW TAIWAN DOLLAR",
        "ILS" => "ISRAELI NEW SHEKEL",
        "AED" => "UAE DIRHAM",
        "QAR" => "QATARI RIAL",
        "KWD" => "KUWAITI DINAR",
        "BHD" => "BAHRAINI DINAR",
        "OMR" => "OMANI RIAL",
        "JOD" => "JORDANIAN DINAR",
        "LBP" => "LEBANESE POUND",
        "EGP" => "EGYPTIAN POUND",
        "IRR" => "IRANIAN RIAL",
        "PKR" => "PAKISTANI RUPEE",
        "BDT" => "BANGLADESHI TAKA",
        "LKR" => "SRI LANKAN RUPEE",
        "NPR" => "NEPALESE RUPEE",
        "MMK" => "MYANMAR KYAT",
        "KHR" => "CAMBODIAN RIEL",
        "LAK" => "LAOTIAN KIP",
        "VND" => "VIETNAMESE DONG",
        "MNT" => "MONGOLIAN TUGRIK",
        "KZT" => "KAZAKHSTANI TENGE",
        "UZS" => "UZBEKISTAN SUM",
        "TJS" => "TAJIKISTANI SOMONI",
        "TMT" => "TURKMENISTAN MANAT",
        "GEL" => "GEORGIAN LARI",
        "AMD" => "ARMENIAN DRAM",
        "AZN" => "AZERBAIJANI MANAT",
        "MDL" => "MOLDOVAN LEU",
        "UAH" => "UKRAINIAN HRYVNIA",
        "BYN" => "BELARUSIAN RUBLE",
        "KGS" => "KYRGYZSTANI SOM",
        "XAU" => "GOLD (OUNCE)",
        "XAG" => "SILVER (OUNCE)",
        "XPT" => "PLATINUM (OUNCE)",
        "XPD" => "PALLADIUM (OUNCE)",
        _ => currencyCode
      };
    }
  }
}