using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.TenantRoleGroups.AddRolesToTenantRoleGroup;

public record AddRolesToTenantRoleGroupCommand(Guid RoleGroupId, List<Guid> RoleIds) : ICommand<AddRolesToTenantRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record AddRolesToTenantRoleGroupResult(bool Success);

public class AddRolesToTenantRoleGroupCommandValidator : AbstractValidator<AddRolesToTenantRoleGroupCommand>
{
  public AddRolesToTenantRoleGroupCommandValidator()
  {
    RuleFor(x => x.RoleGroupId)
        .NotEmpty()
        .WithMessage("Role group ID is required");

    RuleFor(x => x.RoleIds)
        .NotEmpty()
        .WithMessage("At least one role ID is required");
  }
}

public class AddRolesToTenantRoleGroupHandler(
    UserDbContext context,
    ILocalizationService localizationService,
    ILogger<AddRolesToTenantRoleGroupHandler> logger) : ICommandHandler<AddRolesToTenantRoleGroupCommand, AddRolesToTenantRoleGroupResult>
{
  public async Task<AddRolesToTenantRoleGroupResult> Handle(AddRolesToTenantRoleGroupCommand request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupItems)
        .FirstOrDefaultAsync(x => x.Id == request.RoleGroupId, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"Role group with ID {request.RoleGroupId} not found", request.RoleGroupId);
    }

    // Verify all roles exist
    var existingRoles = await context.AppRoles
        .Where(ar => request.RoleIds.Contains(ar.Id))
        .ToListAsync(cancellationToken);

    if (existingRoles.Count != request.RoleIds.Count)
    {
      throw new RoleGroupBadRequestException("One or more roles not found");
    }

    // Check for duplicate role assignments
    var existingRoleIds = roleGroup.RoleGroupItems.Select(ri => ri.AppRoleId).ToList();
    var newRoleIds = request.RoleIds.Except(existingRoleIds).ToList();

    if (!newRoleIds.Any())
    {
      throw new RoleGroupBadRequestException("All specified roles are already assigned to this role group");
    }

    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      foreach (var roleId in newRoleIds)
      {
        var roleGroupItem = await RoleGroupItem.Create(roleGroup.Id, roleId);
        context.RoleGroupItems.Add(roleGroupItem);
      }

      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new AddRolesToTenantRoleGroupResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to add roles to tenant role group. RoleGroupId: {RoleGroupId}", request.RoleGroupId);
      await transaction.RollbackAsync(cancellationToken);
      throw new RoleGroupBadRequestException(await localizationService.Translate("FailedToAddRolesToRoleGroup"));
    }
  }
}