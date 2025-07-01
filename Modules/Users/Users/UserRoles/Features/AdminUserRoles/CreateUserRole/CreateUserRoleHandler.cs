using Users.Data;
using Users.UserRoles.Constants;
using Users.UserRoles.Dtos;
using Users.UserRoles.Exceptions;
using Users.UserRoles.Models;

namespace Users.UserRoles.Features.CreateUserRole;

public record CreateUserRoleCommand(UserRoleDto UserRole) : ICommand<CreateUserRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record CreateUserRoleResult(Guid Id);

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
{
  public CreateUserRoleCommandValidator()
  {
    RuleFor(x => x.UserRole.ManagerId)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.UserIdRequired);

    RuleFor(x => x.UserRole.RoleId)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.RoleIdRequired);
  }
}

public class CreateUserRoleHandler(UserDbContext dbContext) : ICommandHandler<CreateUserRoleCommand, CreateUserRoleResult>
{
  public async Task<CreateUserRoleResult> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
  {
    var existingUserRole = await dbContext.UserRoles
        .FirstOrDefaultAsync(x => x.ManagerId == request.UserRole.ManagerId && x.RoleId == request.UserRole.RoleId, cancellationToken);

    if (existingUserRole != null)
    {
      throw new UserRoleAlreadyExistsException(UserRoleConstants.Operations.UserRoleAlreadyExists);
    }

    var userRole = CreateNewUserRole(request.UserRole);
    await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
    return new CreateUserRoleResult(userRole.Id);
  }

  private UserRole CreateNewUserRole(UserRoleDto userRoleDto)
  {
    var userRole = UserRole.Create(
        userRoleDto.ManagerId,
        userRoleDto.RoleId
    );
    return userRole;
  }
}