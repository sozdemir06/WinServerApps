using FluentValidation;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.TenantRoleGroups.UpdateTenantRoleGroup;

public record UpdateTenantRoleGroupCommand(Guid Id, RoleGroupDto RoleGroup) : ICommand<UpdateTenantRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record UpdateTenantRoleGroupResult(bool Success);

public class UpdateTenantRoleGroupCommandValidator : AbstractValidator<UpdateTenantRoleGroupCommand>
{
  public UpdateTenantRoleGroupCommandValidator()
  {
    RuleFor(x => x.RoleGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.RoleGroup.ViewPermission)
        .IsInEnum()
        .WithMessage("Invalid view permission.");
  }
}

public class UpdateTenantRoleGroupHandler(
    UserDbContext context,
    ILocalizationService localizationService,
    ILogger<UpdateTenantRoleGroupHandler> logger) : ICommandHandler<UpdateTenantRoleGroupCommand, UpdateTenantRoleGroupResult>
{
  public async Task<UpdateTenantRoleGroupResult> Handle(UpdateTenantRoleGroupCommand request, CancellationToken cancellationToken)
  {
    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupTranslatates)
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"Role group with ID {request.Id} not found", request.Id);
    }

    if (request.RoleGroup.Translates.Count == 0)
    {
      throw new RoleGroupBadRequestException("At least one translation is required.");
    }

    var validTranslations = request.RoleGroup.Translates.Where(t => !string.IsNullOrWhiteSpace(t.Name)).ToList();

    // Check for existing role group names (excluding current role group)
    var existingRoleGroupNames = validTranslations.Select(t => t.Name).ToList();
    var existingRoleGroups = await context.RoleGroups
        .Where(rg => rg.Id != request.Id && rg.RoleGroupTranslatates.Any(rt => existingRoleGroupNames.Contains(rt.Name)))
        .ToListAsync(cancellationToken);

    if (existingRoleGroups.Any())
    {
      throw new RoleGroupBadRequestException("A role group with one of the provided names already exists.");
    }

    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      // Update translations
      context.RoleGroupTranslatates.RemoveRange(roleGroup.RoleGroupTranslatates);

      foreach (var transDto in validTranslations)
      {
        var newTranslate = RoleGroupTranslatate.Create(transDto.Name, transDto.Description, transDto.LanguageId, roleGroup.Id);
        context.RoleGroupTranslatates.Add(newTranslate);
      }

      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateTenantRoleGroupResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to update tenant role group. Id: {Id}", request.Id);
      await transaction.RollbackAsync(cancellationToken);
      throw new RoleGroupBadRequestException(await localizationService.Translate("FailedToUpdateRoleGroup"));
    }
  }
}