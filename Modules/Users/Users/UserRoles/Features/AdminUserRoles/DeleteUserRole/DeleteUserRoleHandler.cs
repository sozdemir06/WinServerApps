using Users.Data;
using Users.UserRoles.Constants;
using Users.UserRoles.Exceptions;

namespace Users.UserRoles.Features.DeleteUserRole;

public record DeleteUserRoleCommand(Guid Id) : ICommand<DeleteUserRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record DeleteUserRoleResult(bool Success);

public class DeleteUserRoleHandler(UserDbContext dbContext) : ICommandHandler<DeleteUserRoleCommand, DeleteUserRoleResult>
{
  public async Task<DeleteUserRoleResult> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
  {
    var userRole = await dbContext.UserRoles.FindAsync([request.Id], cancellationToken);

    if (userRole == null)
      throw new UserRoleNotFoundException(string.Format(UserRoleConstants.Operations.UserRoleNotFound, request.Id));

    userRole.IsDeleted = true;
    await dbContext.SaveChangesAsync(cancellationToken);

    return new DeleteUserRoleResult(true);
  }
}