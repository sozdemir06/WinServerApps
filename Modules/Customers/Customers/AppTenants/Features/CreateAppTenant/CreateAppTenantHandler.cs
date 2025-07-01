
namespace Customers.AppTenants.Features.CreateAppTenant;

public record CreateAppTenantCommand(AppTenantDto AppTenant) : ICommand<CreateAppTenantResult>;

public record CreateAppTenantResult(bool Success);

public class CreateAppTenantCommandValidator : AbstractValidator<CreateAppTenantCommand>
{
  public CreateAppTenantCommandValidator()
  {
    RuleFor(x => x.AppTenant.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.AppTenant.Host).NotEmpty().WithMessage("Host is required");
    RuleFor(x => x.AppTenant.TenantCode).NotEmpty().WithMessage("TenantCode is required");
  }
}

public class CreateAppTenantHandler(CustomerDbContext dbContext,ILocalizationService localizationService) : ICommandHandler<CreateAppTenantCommand, CreateAppTenantResult>
{
  public async Task<CreateAppTenantResult> Handle(CreateAppTenantCommand request, CancellationToken cancellationToken)
  {
    var existingTenant = await dbContext.AppTenants
        .FirstOrDefaultAsync(x => x.TenantCode == request.AppTenant.TenantCode, cancellationToken);

    if (existingTenant != null)
    {
      throw new AppTenantNotFoundException(await localizationService.Translate("AlreadyExistsMessage"));
    }

    var appTenant = CreateNewAppTenant(request.AppTenant);
    await dbContext.AppTenants.AddAsync(appTenant, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreateAppTenantResult(true);
  }

  private AppTenant CreateNewAppTenant(AppTenantDto appTenantDto)
  {
    var appTenant = AppTenant.Create(
        appTenantDto.Id,
        appTenantDto.Name,
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
        appTenantDto.MaxUserCount);

    return appTenant;
  }
}