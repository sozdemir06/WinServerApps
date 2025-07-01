using FluentValidation;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.AddRolesToRoleGroup;

public record AddRolesToRoleGroupCommand(Guid RoleGroupId, List<Guid> AppRoleIds) : ICommand<AddRolesToRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record AddRolesToRoleGroupResult(Guid RoleGroupId, int AddedRolesCount);

public class AddRolesToRoleGroupCommandValidator : AbstractValidator<AddRolesToRoleGroupCommand>
{
  public AddRolesToRoleGroupCommandValidator()
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

public class AddRolesToRoleGroupHandler(UserDbContext context) : ICommandHandler<AddRolesToRoleGroupCommand, AddRolesToRoleGroupResult>
{
  public async Task<AddRolesToRoleGroupResult> Handle(AddRolesToRoleGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.AppRoleIds.Count == 0)
    {
      throw new RoleGroupBadRequestException("At least one role must be provided.");
    }

    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupItems)
        .FirstOrDefaultAsync(rg => rg.Id == request.RoleGroupId , cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"RoleGroup with ID '{request.RoleGroupId}' not found.", request.RoleGroupId);
    }

    // Check if all AppRoleIds exist
    var existingAppRoles = await context.AppRoles
        .Where(ar => request.AppRoleIds.Contains(ar.Id) && !ar.IsDeleted)
        .Select(ar => ar.Id)
        .ToListAsync(cancellationToken);

    if (existingAppRoles.Count != request.AppRoleIds.Count)
    {
      var missingRoleIds = request.AppRoleIds.Except(existingAppRoles).ToList();
      throw new RoleGroupBadRequestException($"The following role IDs do not exist: {string.Join(", ", missingRoleIds)}");
    }

    // Check for duplicate role assignments
    var existingRoleGroupItems = roleGroup.RoleGroupItems
        .Where(rgi => request.AppRoleIds.Contains(rgi.AppRoleId))
        .ToList();

    if (existingRoleGroupItems.Any())
    {
      var duplicateRoleIds = existingRoleGroupItems.Select(rgi => rgi.AppRoleId).ToList();
      throw new RoleGroupBadRequestException($"The following roles are already assigned to this role group: {string.Join(", ", duplicateRoleIds)}");
    }

    // Add new role group items
    foreach (var appRoleId in request.AppRoleIds)
    {
      var roleGroupItem = await RoleGroupItem.Create(roleGroup.Id, appRoleId);
      context.RoleGroupItems.Add(roleGroupItem);
    }

    await context.SaveChangesAsync(cancellationToken);

    return new AddRolesToRoleGroupResult(roleGroup.Id, request.AppRoleIds.Count);
  }
}