using WinApps.Modules.Users.Users.AppTenants.Exceptions;

namespace Users.AppTenants.Features.CreateAppTenant;

public record CreateAppTennatCommand(AppTenantDto AppTenant) : ICommand<CreateAppTennatResult>, ICacheRemovingRequest
{
    public List<string> CacheKeysToRemove => [CacheKeys.AppTenants];
}

public record CreateAppTennatResult(Guid Id);

public class CreateAppTenantCommandValidator : AbstractValidator<CreateAppTennatCommand>
{
    public CreateAppTenantCommandValidator()
    {
        RuleFor(x => x.AppTenant)
            .NotNull()
            .WithMessage("AppTenant data is required");

        When(x => x.AppTenant != null, () =>
        {
            RuleFor(x => x.AppTenant.Name)
                .NotEmpty()
                .WithMessage("Name is required"); 

            RuleFor(x => x.AppTenant.Host)
                .NotEmpty()
                .WithMessage("Host is required");

            RuleFor(x => x.AppTenant.TenantCode)
                .NotEmpty()
                .WithMessage("TenantCode is required");
        });
    }
}

public class CreateAppTenantHandler(UserDbContext dbContext) : ICommandHandler<CreateAppTennatCommand, CreateAppTennatResult>
{
    public async Task<CreateAppTennatResult> Handle(CreateAppTennatCommand request, CancellationToken cancellationToken)
    {

       

        var checkAppTenant = await dbContext.AppTenants.FirstOrDefaultAsync(x => x.TenantCode == request.AppTenant.TenantCode, cancellationToken);
        if (checkAppTenant != null)
        {
            throw new AppTenantAlreadyExistsException(request.AppTenant.TenantCode);
        }
        var appTenant = CreateNewAppTenant(request.AppTenant);
        await dbContext.AppTenants.AddAsync(appTenant, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateAppTennatResult(appTenant.Id);
    }


    private AppTenant CreateNewAppTenant(AppTenantDto appTenantDto)
    {
        var appTenant = AppTenant
            .Create(
            appTenantDto.Name ?? string.Empty,
            appTenantDto.Host,
            appTenantDto.Phone,
            appTenantDto.TenantCode,
            appTenantDto.IsEnabledWebUi,
            appTenantDto.Description,
            appTenantDto.AdminEmail,
            appTenantDto.AllowedBranchNumber,
            appTenantDto.IsActive,
            appTenantDto.SubscriptionEndDate,
            appTenantDto.SubscriptionStartDate,
            appTenantDto.TenantType,
            appTenantDto.MaxUserCount
           );
        return appTenant;
    }
}
