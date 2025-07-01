using Shared.Services.Securities;
using Users.AppRoles.Models;
using Users.Languages.Models;
using Users.Managers.Models;

namespace Users.Data.Seed;

public static class InitialData
{


    public static IEnumerable<AppRole> GetAppRoles()
    {
        return [
            AppRole.Create(
                name: "Admin",
                description: "Admin role for system management",
                roleLanguageKey: RoleLanguageKey.Approve
            ),
        ];
    }

    public static IEnumerable<AppTenant> GetAppTenants()
    {

       
        return
        [
            AppTenant.Create(
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

    public static IEnumerable<Manager> GetManagers()
    {

        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePaswordHash("Pa$$W0rd", out passwordHash, out passwordSalt);

        // Create a default admin manager
        var adminManager = Manager.Create(
            userName: "WinApps",
            email: "ahmetdurmaz@gmail.com",
            phoneNumber: string.Empty,
            firstName: "Ahmet",
            lastName: "Durmaz",
            photoUrl: string.Empty,
            normalizedUserName: "WINAPPS",
            normalizedEmail: "AHMETDURMAZ@GMAIL.COM",
            isAdmin: true,
            isManager: true,
            tenantId: Guid.Parse("019731d6-5e4e-7f73-b517-bf6fcd4442ea"),
            passwordHash: passwordHash,
            passwordSalt: passwordSalt
        );

        return [adminManager];
    }

    public static IEnumerable<Language> GetLanguages()
    {
        return [
            Language.Create(name: "English", code: "en", description: "English language", isDefault: true, isActive: true),
            Language.Create(name: "Turkish", code: "tr", description: "Turkish language", isDefault: false, isActive: true),
        ];
    }
}