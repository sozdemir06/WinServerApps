namespace Modules.Users.Users.AppTenants.Features.UpdateAppTenant;

public record UpdateAppTenantCommand(Guid Id, AppTenantDto AppTenant) : ICommand<UpdateAppTenantResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AppTenants];
}

public record UpdateAppTenantResult(bool Success);

public class UpdateAppTenantCommandValidator : AbstractValidator<UpdateAppTenantCommand>
{
  public UpdateAppTenantCommandValidator()
  {
    RuleFor(x => x.AppTenant.Name)
        .NotEmpty()
        .WithMessage(AppTenantErrorMessages.Validation.AppTenantNameRequired);
    RuleFor(x => x.AppTenant.Host)
        .NotEmpty()
        .WithMessage(AppTenantErrorMessages.Validation.AppTenantHostRequired);
    RuleFor(x => x.AppTenant.TenantCode)
        .NotEmpty()
        .WithMessage(AppTenantErrorMessages.Validation.AppTenantTenantCodeRequired);

  }
}

public class UpdateAppTenantHandler(
  UserDbContext dbContext,
  ILocalizationService localizationService,
  ILogger<UpdateAppTenantHandler> logger
  ) : ICommandHandler<UpdateAppTenantCommand, UpdateAppTenantResult>
{
  public async Task<UpdateAppTenantResult> Handle(UpdateAppTenantCommand request, CancellationToken cancellationToken)
  {
    var appTenant = await dbContext.AppTenants.FindAsync([request.Id], cancellationToken);

    if (appTenant == null)
    {
      throw new AppTenantNotFoundException(await localizationService.Translate("NotFoundMessage"));
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      UpdateAppTenant(appTenant, request.AppTenant);

      var appTenantUpdatedEvent = appTenant.Adapt<AppTenantUpdatedIntegrationEvent>();

      var outboxMessage = new OutboxMessage
      {
        Id = Guid.CreateVersion7(),
        CreatedAt = DateTime.UtcNow,
        Type = typeof(AppTenantUpdatedIntegrationEvent).AssemblyQualifiedName!,
        Content = JsonSerializer.Serialize(appTenantUpdatedEvent)
      };

      await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateAppTenantResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex,
        "Failed to update app tenant. AppTenantId: {AppTenantId}, Name: {AppTenantName}",
        request.AppTenant.Id,
        request.AppTenant.Name
       );

      await transaction.RollbackAsync(cancellationToken);
      throw new AppTenantUpdateException(await localizationService.Translate("UpdateFailed"));
    }


  }

  private void UpdateAppTenant(AppTenant appTenant, AppTenantDto appTenantDto)
  {
    appTenant.Update(
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
        appTenantDto.MaxUserCount
    );
  }
}