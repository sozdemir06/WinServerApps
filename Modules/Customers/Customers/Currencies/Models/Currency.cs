using Customers.Currencies.DomainEvents;
using Shared.DDD;

namespace Customers.Currencies.Models;

public class Currency : Aggregate<long>
{
    public string? CurrencyCode { get; set; } = string.Empty;
    public string? CurrencyName { get; set; } = string.Empty;
    public decimal? ForexBuying { get; set; } = 0;
    public decimal? ForexSelling { get; set; } = 0;
    public decimal? BanknoteBuying { get; set; } = 0;
    public decimal? BanknoteSelling { get; set; } = 0;
    public DateTime? Date { get; set; }

    private Currency()
    {
        // For EF Core
    }

    public static Currency Create(
        long id,
        string currencyCode,
        string currencyName,
        decimal? forexBuying = 0,
        decimal? forexSelling = 0,
        decimal? banknoteBuying = 0,
        decimal? banknoteSelling = 0)
    {
        var currency = new Currency
        {
            Id = id,
            CreatedAt = DateTime.UtcNow,
            CurrencyCode = currencyCode,
            CurrencyName = currencyName,
            ForexBuying = forexBuying,
            ForexSelling = forexSelling,
            BanknoteBuying = banknoteBuying,
            BanknoteSelling = banknoteSelling,
            Date = DateTime.UtcNow
        };

        currency.AddDomainEvent(new CurrencyCreatedEvent(currency));
        return currency;
    }

    public void Update(
        string currencyCode,
        string currencyName,
        decimal? forexBuying = null,
        decimal? forexSelling = null,
        decimal? banknoteBuying = null,
        decimal? banknoteSelling = null)
    {
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;

        if (forexBuying.HasValue)
            ForexBuying = forexBuying;
        if (forexSelling.HasValue)
            ForexSelling = forexSelling;
        if (banknoteBuying.HasValue)
            BanknoteBuying = banknoteBuying;
        if (banknoteSelling.HasValue)
            BanknoteSelling = banknoteSelling;

        AddDomainEvent(new CurrencyUpdatedEvent(this));
    }

    public void UpdateExchangeRate(decimal? forexBuying, decimal? forexSelling, decimal? banknoteBuying, decimal? banknoteSelling)
    {
        ForexBuying = forexBuying;
        ForexSelling = forexSelling;
        BanknoteBuying = banknoteBuying;
        BanknoteSelling = banknoteSelling;
        Date = DateTime.UtcNow;
        AddDomainEvent(new CurrencyExchangeRateUpdatedEvent(this));
    }

    public void Activate()
    {
        // Since there's no IsActive property, we'll use a different approach
        // We can consider a currency active if it has a valid code and name
        if (string.IsNullOrEmpty(CurrencyCode) || string.IsNullOrEmpty(CurrencyName))
        {
            // This would be a business rule - you might want to add an IsActive property
            // For now, we'll just add the event
            AddDomainEvent(new CurrencyActivatedEvent(this));
        }
    }

    public void Deactivate()
    {
        // Since there's no IsActive property, we'll clear the rates to indicate deactivation
        ForexBuying = 0;
        ForexSelling = 0;
        BanknoteBuying = 0;
        BanknoteSelling = 0;
        AddDomainEvent(new CurrencyDeactivatedEvent(this));
    }
}