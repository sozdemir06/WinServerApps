using Users.Data;
using Users.UserRoles.Constants;
using Users.UserRoles.Exceptions;
using Users.UserRoles.Models;

namespace Users.UserRoles.Features.RemoveUserRoles;

public record RemoveUserRolesCommand(List<Guid> UserRoleIds) : ICommand<RemoveUserRolesResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record RemoveUserRolesResult(int RemovedCount, int NotFoundCount, List<Guid> RemovedIds);

public class RemoveUserRolesCommandValidator : AbstractValidator<RemoveUserRolesCommand>
{
  public RemoveUserRolesCommandValidator()
  {
    RuleFor(x => x.UserRoleIds)
        .NotEmpty()
        .WithMessage("At least one user role ID must be provided.");

    RuleForEach(x => x.UserRoleIds)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.UserIdRequired);
  }
}

public class RemoveUserRolesHandler(UserDbContext dbContext) : ICommandHandler<RemoveUserRolesCommand, RemoveUserRolesResult>
{
  public async Task<RemoveUserRolesResult> Handle(RemoveUserRolesCommand request, CancellationToken cancellationToken)
  {
    var removedIds = new List<Guid>();
    var notFoundCount = 0;

    foreach (var userRoleId in request.UserRoleIds)
    {
      var userRole = await dbContext.UserRoles
          .FirstOrDefaultAsync(x => x.Id == userRoleId, cancellationToken);

      if (userRole == null)
      {
        notFoundCount++;
        continue;
      }

      dbContext.UserRoles.Remove(userRole);
      removedIds.Add(userRoleId);
    }

    await dbContext.SaveChangesAsync(cancellationToken);

    return new RemoveUserRolesResult(removedIds.Count, notFoundCount, removedIds);
  }
}