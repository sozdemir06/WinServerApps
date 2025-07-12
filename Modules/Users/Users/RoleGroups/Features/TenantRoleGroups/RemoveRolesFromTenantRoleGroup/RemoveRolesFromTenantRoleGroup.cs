using Users.RoleGroups.Exceptions;

namespace Users.RoleGroups.Features.TenantRoleGroups.RemoveRolesFromTenantRoleGroup;

public record RemoveRolesFromTenantRoleGroupCommand(Guid RoleGroupId, List<Guid> RoleIds) : ICommand<RemoveRolesFromTenantRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record RemoveRolesFromTenantRoleGroupResult(bool Success);

public class RemoveRolesFromTenantRoleGroupCommandValidator : AbstractValidator<RemoveRolesFromTenantRoleGroupCommand>
{
  public RemoveRolesFromTenantRoleGroupCommandValidator()
  {
    RuleFor(x => x.RoleGroupId)
        .NotEmpty()
        .WithMessage("Role group ID is required");

    RuleFor(x => x.RoleIds)
        .NotEmpty()
        .WithMessage("At least one role ID is required");
  }
}

public class RemoveRolesFromTenantRoleGroupHandler(
    UserDbContext context,
    ILocalizationService localizationService,
    ILogger<RemoveRolesFromTenantRoleGroupHandler> logger) : ICommandHandler<RemoveRolesFromTenantRoleGroupCommand, RemoveRolesFromTenantRoleGroupResult>
{
  public async Task<RemoveRolesFromTenantRoleGroupResult> Handle(RemoveRolesFromTenantRoleGroupCommand request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupItems)
        .FirstOrDefaultAsync(x => x.Id == request.RoleGroupId, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"Role group with ID {request.RoleGroupId} not found", request.RoleGroupId);
    }

    // Find existing role group items to remove
    var existingRoleGroupItems = roleGroup.RoleGroupItems
        .Where(rgi => request.RoleIds.Contains(rgi.AppRoleId))
        .ToList();

    if (!existingRoleGroupItems.Any())
    {
      throw new RoleGroupBadRequestException("None of the specified roles are assigned to this role group");
    }

    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      context.RoleGroupItems.RemoveRange(existingRoleGroupItems);
      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new RemoveRolesFromTenantRoleGroupResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to remove roles from tenant role group. RoleGroupId: {RoleGroupId}", request.RoleGroupId);
      await transaction.RollbackAsync(cancellationToken);
      throw new RoleGroupBadRequestException(await localizationService.Translate("FailedToRemoveRolesFromRoleGroup"));
    }
  }
}