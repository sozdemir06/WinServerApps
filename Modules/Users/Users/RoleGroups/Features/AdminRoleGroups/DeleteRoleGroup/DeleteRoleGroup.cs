using FluentValidation;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.DeleteRoleGroup;

public record DeleteRoleGroupCommand(Guid Id) : ICommand<DeleteRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups, $"{CacheKeys.RoleGroups}:{Id}"];
}

public record DeleteRoleGroupResult(Guid Id);

public class DeleteRoleGroupCommandValidator : AbstractValidator<DeleteRoleGroupCommand>
{
  public DeleteRoleGroupCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("RoleGroup ID is required.");
  }
}

public class DeleteRoleGroupHandler(UserDbContext context) : ICommandHandler<DeleteRoleGroupCommand, DeleteRoleGroupResult>
{
  public async Task<DeleteRoleGroupResult> Handle(DeleteRoleGroupCommand request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .FirstOrDefaultAsync(rg => rg.Id == request.Id && !rg.IsDeleted, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"RoleGroup with ID '{request.Id}' not found.", request.Id);
    }

    roleGroup.Deactivate();

    await context.SaveChangesAsync(cancellationToken);

    return new DeleteRoleGroupResult(roleGroup.Id);
  }
}