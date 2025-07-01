using FluentValidation;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.RemoveRolesFromRoleGroup;

public record RemoveRolesFromRoleGroupCommand(Guid RoleGroupId, List<Guid> AppRoleIds) : ICommand<RemoveRolesFromRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record RemoveRolesFromRoleGroupResult(Guid RoleGroupId, int RemovedRolesCount);

public class RemoveRolesFromRoleGroupCommandValidator : AbstractValidator<RemoveRolesFromRoleGroupCommand>
{
  public RemoveRolesFromRoleGroupCommandValidator()
  {
    RuleFor(x => x.RoleGroupId)
      .NotEmpty()
      .WithMessage("RoleGroup ID is required.");

    RuleFor(x => x.AppRoleIds)
      .NotEmpty()
      .WithMessage("At least one role must be provided.");

    RuleForEach(x => x.AppRoleIds)
      .NotEmpty()
      .WithMessage("AppRole ID cannot be empty.");
  }
}

public class RemoveRolesFromRoleGroupHandler(UserDbContext context) : ICommandHandler<RemoveRolesFromRoleGroupCommand, RemoveRolesFromRoleGroupResult>
{
  public async Task<RemoveRolesFromRoleGroupResult> Handle(RemoveRolesFromRoleGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.AppRoleIds.Count == 0)
    {
      throw new RoleGroupBadRequestException("At least one role must be provided.");
    }

    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupItems)
        .FirstOrDefaultAsync(rg => rg.Id == request.RoleGroupId && !rg.IsDeleted, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"RoleGroup with ID '{request.RoleGroupId}' not found.", request.RoleGroupId);
    }

    // Find existing role group items to remove
    var roleGroupItemsToRemove = roleGroup.RoleGroupItems
        .Where(rgi => request.AppRoleIds.Contains(rgi.AppRoleId))
        .ToList();

    if (!roleGroupItemsToRemove.Any())
    {
      throw new RoleGroupBadRequestException("None of the specified roles are assigned to this role group.");
    }

    // Check if all requested roles exist in the role group
    var existingAppRoleIds = roleGroupItemsToRemove.Select(rgi => rgi.AppRoleId).ToList();
    var missingRoleIds = request.AppRoleIds.Except(existingAppRoleIds).ToList();

    if (missingRoleIds.Any())
    {
      throw new RoleGroupBadRequestException($"The following roles are not assigned to this role group: {string.Join(", ", missingRoleIds)}");
    }

    // Remove role group items
    context.RoleGroupItems.RemoveRange(roleGroupItemsToRemove);

    await context.SaveChangesAsync(cancellationToken);

    return new RemoveRolesFromRoleGroupResult(roleGroup.Id, roleGroupItemsToRemove.Count);
  }
}