using FluentValidation;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.TenantRoleGroups.CreateTenantRoleGroup;

public record CreateTenantRoleGroupCommand(RoleGroupDto RoleGroup) : ICommand<CreateTenantRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record CreateTenantRoleGroupResult(Guid Id);

public class CreateTenantRoleGroupCommandValidator : AbstractValidator<CreateTenantRoleGroupCommand>
{
  public CreateTenantRoleGroupCommandValidator()
  {
    RuleFor(x => x.RoleGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.RoleGroup.ViewPermission)
        .IsInEnum()
        .WithMessage("Invalid view permission.");
  }
}

public class CreateTenantRoleGroupHandler(
    UserDbContext context,
    ILocalizationService localizationService,
    ILogger<CreateTenantRoleGroupHandler> logger) : ICommandHandler<CreateTenantRoleGroupCommand, CreateTenantRoleGroupResult>
{
  public async Task<CreateTenantRoleGroupResult> Handle(CreateTenantRoleGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.RoleGroup.Translates.Count == 0)
    {
      throw new RoleGroupBadRequestException("At least one translation is required.");
    }

    var validTranslations = request.RoleGroup.Translates.Where(t => !string.IsNullOrWhiteSpace(t.Name)).ToList();

    // Check for existing role group names
    var existingRoleGroupNames = validTranslations.Select(t => t.Name).ToList();
    var existingRoleGroups = await context.RoleGroups
        .Where(rg => rg.RoleGroupTranslatates.Any(rt => existingRoleGroupNames.Contains(rt.Name)))
        .ToListAsync(cancellationToken);

    if (existingRoleGroups.Any())
    {
      throw new RoleGroupBadRequestException("A role group with one of the provided names already exists.");
    }

    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      var roleGroup = await RoleGroup.Create(validTranslations);

      await context.RoleGroups.AddAsync(roleGroup, cancellationToken);
      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new CreateTenantRoleGroupResult(roleGroup.Id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to create tenant role group");
      await transaction.RollbackAsync(cancellationToken);
      throw new RoleGroupBadRequestException(await localizationService.Translate("FailedToCreateRoleGroup"));
    }
  }
}