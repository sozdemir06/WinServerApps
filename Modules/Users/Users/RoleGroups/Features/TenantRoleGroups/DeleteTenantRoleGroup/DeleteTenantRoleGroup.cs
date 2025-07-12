using Users.RoleGroups.Exceptions;

namespace Users.RoleGroups.Features.TenantRoleGroups.DeleteTenantRoleGroup;

public record DeleteTenantRoleGroupCommand(Guid Id) : ICommand<DeleteTenantRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record DeleteTenantRoleGroupResult(bool Success);

public class DeleteTenantRoleGroupHandler(
    UserDbContext context,
    ILocalizationService localizationService,
    ILogger<DeleteTenantRoleGroupHandler> logger) : ICommandHandler<DeleteTenantRoleGroupCommand, DeleteTenantRoleGroupResult>
{
  public async Task<DeleteTenantRoleGroupResult> Handle(DeleteTenantRoleGroupCommand request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"Role group with ID {request.Id} not found", request.Id);
    }

    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      roleGroup.Deactivate();
      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new DeleteTenantRoleGroupResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to delete tenant role group. Id: {Id}", request.Id);
      await transaction.RollbackAsync(cancellationToken);
      throw new RoleGroupBadRequestException(await localizationService.Translate("FailedToDeleteRoleGroup"));
    }
  }
}