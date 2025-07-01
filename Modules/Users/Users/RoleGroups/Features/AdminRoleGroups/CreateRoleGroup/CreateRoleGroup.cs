using FluentValidation;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.CreateRoleGroup;

public record CreateRoleGroupCommand(RoleGroupDto RoleGroup) : ICommand<CreateRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record CreateRoleGroupResult(Guid Id);

public class CreateRoleGroupCommandValidator : AbstractValidator<CreateRoleGroupCommand>
{
  public CreateRoleGroupCommandValidator()
  {

    RuleFor(x => x.RoleGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.RoleGroup.ViewPermission)
        .IsInEnum()
        .WithMessage("Invalid view permission.");
  }
}

public class CreateRoleGroupHandler(UserDbContext context) : ICommandHandler<CreateRoleGroupCommand, CreateRoleGroupResult>
{
  public async Task<CreateRoleGroupResult> Handle(CreateRoleGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.RoleGroup.Translates.Count == 0)
    {
      throw new RoleGroupBadRequestException("At least one translation is required.");
    }

    var validTranslations = request.RoleGroup.Translates.Where(t => !string.IsNullOrWhiteSpace(t.Name)).ToList();

    // Sadece geçerli çevirilerin isimlerini kontrol et
    var existingRoleGroupNames = validTranslations.Select(t => t.Name).ToList();
    var existingRoleGroups = await context.RoleGroups
        .Where(rg => rg.RoleGroupTranslatates.Any(rt => existingRoleGroupNames.Contains(rt.Name)))
        .ToListAsync(cancellationToken);

    if (existingRoleGroups.Any())
    {
      throw new RoleGroupBadRequestException("A role group with one of the provided names already exists.");
    }

    var roleGroup = await RoleGroup.Create(validTranslations);

    await context.RoleGroups.AddAsync(roleGroup, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return new CreateRoleGroupResult(roleGroup.Id);
  }
}