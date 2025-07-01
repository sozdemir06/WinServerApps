



namespace Customers.AppTenants.Features.UpdateAppTenant;

public record UpdateAppTenantCommand(AppTenantDto AppTenant) : ICommand<UpdateAppTenantResult>;

public record UpdateAppTenantResult(bool Success);

public class UpdateAppTenantCommandValidator : AbstractValidator<UpdateAppTenantCommand>
{
  public UpdateAppTenantCommandValidator()
  {
    RuleFor(x => x.AppTenant.Id).NotEmpty().WithMessage("Id is required");
    RuleFor(x => x.AppTenant.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.AppTenant.Host).NotEmpty().WithMessage("Host is required");
    RuleFor(x => x.AppTenant.TenantCode).NotEmpty().WithMessage("TenantCode is required");
  }
}

public class UpdateAppTenantHandler(CustomerDbContext dbContext) : ICommandHandler<UpdateAppTenantCommand, UpdateAppTenantResult>
{
  public async Task<UpdateAppTenantResult> Handle(UpdateAppTenantCommand request, CancellationToken cancellationToken)
  {
    var tenant = await dbContext.AppTenants
        .FirstOrDefaultAsync(x => x.Id == request.AppTenant.Id || x.TenantCode == request.AppTenant.TenantCode, cancellationToken);

    if (tenant == null)
    {
      // Create new tenant if not exists
      tenant = AppTenant.Create(
          request.AppTenant.Id,
          request.AppTenant.Name,
          request.AppTenant.Host,
          request.AppTenant.Phone,
          request.AppTenant.TenantCode,
          request.AppTenant.IsEnabledWebUi,
          request.AppTenant.Description,
          request.AppTenant.AdminEmail,
          request.AppTenant.AllowedBranchNumber,
          request.AppTenant.IsActive,
          request.AppTenant.SubscriptionEndDate,
          request.AppTenant.SubscriptionStartDate,
          request.AppTenant.TenantType,
          request.AppTenant.MaxUserCount);

      await dbContext.AppTenants.AddAsync(tenant, cancellationToken);
    }
    else
    {
      // Update existing tenant
      tenant.Update(
          request.AppTenant.Name,
          request.AppTenant.Host,
          request.AppTenant.Phone,
          request.AppTenant.TenantCode,
          request.AppTenant.IsEnabledWebUi,
          request.AppTenant.Description,
          request.AppTenant.AdminEmail,
          request.AppTenant.AllowedBranchNumber,
          request.AppTenant.IsActive,
          request.AppTenant.SubscriptionEndDate,
          request.AppTenant.SubscriptionStartDate,
          request.AppTenant.TenantType,
          request.AppTenant.MaxUserCount);
    }

    await dbContext.SaveChangesAsync(cancellationToken);
    return new UpdateAppTenantResult(true);
  }
}