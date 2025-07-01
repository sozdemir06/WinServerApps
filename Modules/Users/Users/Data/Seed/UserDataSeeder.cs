using Shared.Data.Seed;
using Users.Managers.Models;
using Users.UserRoles.Models;

namespace Users.Data.Seed;

public class UserDataSeeder(UserDbContext _dbContext) : IDataSeeder
{
    public async Task SeedAllAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        if (!await _dbContext.AppTenants.AnyAsync(cancellationToken))
        {
            await _dbContext.AppTenants.AddRangeAsync(InitialData.GetAppTenants(), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }



        if (!await _dbContext.Managers.AnyAsync(cancellationToken))
        {

            // Get managers from initial data
            var managers = InitialData.GetManagers();
            var tenantId = await _dbContext.AppTenants.FirstOrDefaultAsync(t => t.TenantCode == "ADMIN", cancellationToken);

            // Check each manager's email before adding
            foreach (var manager in managers)
            {
                var normalizedEmail = manager.Email.ToUpperInvariant();

                // Temporarily disable the tenant filter when checking for existing managers
                var existingManager = await _dbContext.Managers
                    .IgnoreQueryFilters()  // This will ignore the tenant filter
                    .FirstOrDefaultAsync(m => m.NormalizedEmail == normalizedEmail, cancellationToken);

                if (existingManager == null)
                {
                    var newManager = Manager.Create(
                        userName: manager.UserName,
                        email: manager.Email,
                        phoneNumber: manager.PhoneNumber ?? string.Empty, 
                        firstName: manager.FirstName,
                        lastName: manager.LastName,
                        photoUrl: manager.PhotoUrl ?? string.Empty,
                        normalizedUserName: manager.NormalizedUserName,
                        normalizedEmail: normalizedEmail,
                        isAdmin: manager.IsAdmin,
                        isManager: manager.IsManager,
                        passwordHash: manager.PasswordHash,
                        passwordSalt: manager.PasswordSalt,
                        tenantId: tenantId!.Id
                    );
                    await _dbContext.Managers.AddAsync(newManager, cancellationToken);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken); 
        }

        if (!await _dbContext.AppRoles.AnyAsync(cancellationToken))
        {
            await _dbContext.AppRoles.AddRangeAsync(InitialData.GetAppRoles(), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        if (!await _dbContext.Languages.AnyAsync(cancellationToken))
        {
            await _dbContext.Languages.AddRangeAsync(InitialData.GetLanguages(), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        if (!await _dbContext.UserRoles.AnyAsync(cancellationToken))
        {
            var adminRole = await _dbContext.AppRoles.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);
            var manager=await _dbContext.Managers.FirstOrDefaultAsync(m=>m.Email=="ahmetdurmaz@gmail.com",cancellationToken);
            if (adminRole != null && manager != null)
            {
                var useRole=UserRole.Create(
                    managerId: manager.Id,
                    roleId: adminRole.Id
                );
                await _dbContext.UserRoles.AddAsync(useRole, cancellationToken);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
           

     
    }
}