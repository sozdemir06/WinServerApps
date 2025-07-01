using Users.Managers.Exceptions;
using Users.UserRoles.Models;

namespace Users.UserRoles.Features.CreateUserRoles;

public record CreateUserRolesCommand(Guid ManagerId, List<Guid> RoleIds) : ICommand<CreateUserRolesResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles, CacheKeys.Managers];
}

public record CreateUserRolesResult(bool IsSuccess);

public class CreateUserRolesCommandValidator : AbstractValidator<CreateUserRolesCommand>
{
  public CreateUserRolesCommandValidator()
  {
    RuleFor(x => x.RoleIds)
        .NotEmpty()
        .WithMessage("At least one user role must be provided.");

    RuleFor(x => x.ManagerId)
        .NotEmpty()
        .WithMessage("Manager ID is required.");
  }
}

public class CreateUserRolesHandler(UserDbContext dbContext) : ICommandHandler<CreateUserRolesCommand, CreateUserRolesResult>
{
  public async Task<CreateUserRolesResult> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
                 .IgnoreQueryFilters()
                  .AsNoTracking()
                  .FirstOrDefaultAsync(x => x.Id == request.ManagerId, cancellationToken);
    if (manager == null)
    {
      throw new ManagerNotFoundException(request.ManagerId);
    }

    // Get existing user roles for this manager
    var existingUserRoles = await dbContext.UserRoles
        .IgnoreQueryFilters()
        .Where(x => x.ManagerId == request.ManagerId)
        .ToListAsync(cancellationToken);

    var existingRoleIds = existingUserRoles.Select(x => x.RoleId).ToList();

    // Find roles to add (incoming roles that don't exist)
    var rolesToAdd = request.RoleIds.Except(existingRoleIds).ToList();

    // Find roles to remove (existing roles that are not in incoming list)
    var rolesToRemove = existingRoleIds.Except(request.RoleIds).ToList();

    // Remove roles that are not in the incoming list
    var userRolesToRemove = existingUserRoles.Where(x => rolesToRemove.Contains(x.RoleId)).ToList();
    dbContext.UserRoles.RemoveRange(userRolesToRemove);

    // Create new user roles for roles that don't exist
    foreach (var roleId in rolesToAdd)
    {
      var userRole = CreateNewUserRole(request.ManagerId, roleId);
      await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
    }

    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreateUserRolesResult(true);
  }

  private UserRole CreateNewUserRole(Guid managerId, Guid roleId)
  {
    return UserRole.Create(managerId, roleId);
  }
}