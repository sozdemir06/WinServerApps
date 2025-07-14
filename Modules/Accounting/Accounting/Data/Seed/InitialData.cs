using Accounting.AppTenants.Models;
using Accounting.Currencies.Models;
using Accounting.Languages.Models;

namespace Accounting.Data.Seed;

public static class InitialData
{
  public static IEnumerable<AppTenant> GetAppTenants()
  {
    return
    [
        AppTenant.Create(
                id: Guid.Parse("019731d6-5e4e-7f73-b517-bf6fcd4442ea"),
                name: "Admin Tenant",
                host: "winfiniti.com.tr",
                phone: null,
                tenantCode: "ADMIN",
                isEnabledWebUi: true,
                description: "Admin tenant for system management",
                adminEmail: "ahmetdurmaz@gmail.com",
                allowedBranchNumber: 1,
                isActive: true,
                subscriptionEndDate: DateTime.UtcNow.AddYears(1),
                subscriptionStartDate: DateTime.UtcNow,
                tenantType: "Admin",
                maxUserCount: 10
            ),
    ];
  }

  public static IEnumerable<Currency> GetCurrencies()
  {
    return
    [
        Currency.Create(
            id: 1,
            currencyCode: "TRY",
            currencyName: "Turkish Lira",
            forexBuying: 1.0m,
            forexSelling: 1.0m,
            banknoteBuying: 1.0m,
            banknoteSelling: 1.0m
        ),
        Currency.Create(
            id: 2,
            currencyCode: "USD",
            currencyName: "US Dollar",
            forexBuying: 30.5m,
            forexSelling: 30.8m,
            banknoteBuying: 30.2m,
            banknoteSelling: 31.0m
        ),
        Currency.Create(
            id: 3,
            currencyCode: "EUR",
            currencyName: "Euro",
            forexBuying: 33.2m,
            forexSelling: 33.5m,
            banknoteBuying: 32.9m,
            banknoteSelling: 33.8m
        ),
    ];
  }

  public static IEnumerable<Language> GetLanguages()
  {
    return
    [
        Language.Create(
            id: Guid.Parse("12345678-1234-1234-1234-123456789012"),
            name: "Türkçe",
            code: "tr-TR",
            description: "Turkish language",
            isDefault: true,
            isActive: true
        ),
        Language.Create(
            id: Guid.Parse("87654321-4321-4321-4321-210987654321"),
            name: "English",
            code: "en-US",
            description: "English language",
            isDefault: false,
            isActive: true
        ),
    ];
  }
}