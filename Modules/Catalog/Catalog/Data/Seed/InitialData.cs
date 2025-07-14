using Catalog.AppTenants.Models;
using Catalog.AppUnits.Models;
using Catalog.Currencies.Models;
using Catalog.Languages.Models;

namespace Catalog.Data.Seed;

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

    public static IEnumerable<AppUnit> GetAppUnits()
    {
        return
        [
            AppUnit.Create(
            MeasureUnitType.Quantity,
            isActive: true
        ),
        AppUnit.Create(
            MeasureUnitType.Weight,
            isActive: true
        ),
        AppUnit.Create(
            MeasureUnitType.Length,
            isActive: true
        ),
        AppUnit.Create(
            MeasureUnitType.Volume,
            isActive: true
        ),
    ];
    }

    public static IEnumerable<AppUnitTranslate> GetAppUnitTranslates()
    {
        var trLanguageId = Guid.Parse("12345678-1234-1234-1234-123456789012");
        var enLanguageId = Guid.Parse("87654321-4321-4321-4321-210987654321");

        return
        [
            // Turkish translations
            AppUnitTranslate.Create("Adet", "Birim sayısı", trLanguageId, Guid.Parse("11111111-1111-1111-1111-111111111111")),
        AppUnitTranslate.Create("Piece", "Unit count", enLanguageId, Guid.Parse("11111111-1111-1111-1111-111111111111")),

        AppUnitTranslate.Create("Kilogram", "Ağırlık birimi", trLanguageId, Guid.Parse("22222222-2222-2222-2222-222222222222")),
        AppUnitTranslate.Create("Kilogram", "Weight unit", enLanguageId, Guid.Parse("22222222-2222-2222-2222-222222222222")),

        AppUnitTranslate.Create("Metre", "Uzunluk birimi", trLanguageId, Guid.Parse("33333333-3333-3333-3333-333333333333")),
        AppUnitTranslate.Create("Meter", "Length unit", enLanguageId, Guid.Parse("33333333-3333-3333-3333-333333333333")),

        AppUnitTranslate.Create("Litre", "Hacim birimi", trLanguageId, Guid.Parse("44444444-4444-4444-4444-444444444444")),
        AppUnitTranslate.Create("Liter", "Volume unit", enLanguageId, Guid.Parse("44444444-4444-4444-4444-444444444444")),
    ];
    }
}