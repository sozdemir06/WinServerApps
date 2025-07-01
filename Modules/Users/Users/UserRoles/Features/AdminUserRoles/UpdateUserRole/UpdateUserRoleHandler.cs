using Users.UserRoles.Constants;
using Users.UserRoles.Dtos;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.UpdateUserRole;

public record UpdateUserRoleCommand(UserRoleDto UserRole) : ICommand<UpdateUserRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record UpdateUserRoleResult(bool Success);

public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
  public UpdateUserRoleCommandValidator()
  {
    RuleFor(x => x.UserRole.Id)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Operations.UserRoleInvalidId);

    RuleFor(x => x.UserRole.ManagerId)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.UserIdRequired);

    RuleFor(x => x.UserRole.RoleId)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.RoleIdRequired);
  }
}

public class UpdateUserRoleHandler(
    UserDbContext dbContext
) : ICommandHandler<UpdateUserRoleCommand, UpdateUserRoleResult>
{
  public async Task<UpdateUserRoleResult> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
  {
    var userRole = await dbContext.UserRoles.FindAsync([request.UserRole.Id], cancellationToken) 
    ?? throw new UserRoleNotFoundException(string.Format(UserRoleConstants.Operations.UserRoleNotFound, request.UserRole.Id));
        
    userRole.Update(request.UserRole.IsActive);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new UpdateUserRoleResult(true);
  }
}