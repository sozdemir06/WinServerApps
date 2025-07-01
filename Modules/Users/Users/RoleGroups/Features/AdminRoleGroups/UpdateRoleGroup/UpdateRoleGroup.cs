using FluentValidation;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Exceptions;
using Users.RoleGroups.models;

namespace Users.RoleGroups.Features.UpdateRoleGroup;

public record UpdateRoleGroupCommand(Guid Id, RoleGroupDto RoleGroup) : ICommand<UpdateRoleGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.RoleGroups];
}

public record UpdateRoleGroupResult(Guid Id);

public class UpdateRoleGroupCommandValidator : AbstractValidator<UpdateRoleGroupCommand>
{
  public UpdateRoleGroupCommandValidator()
  {

    RuleFor(x => x.RoleGroup)
        .NotNull()
        .WithMessage("RoleGroup data is required.");

    RuleFor(x => x.RoleGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

  }
}

public class UpdateRoleGroupHandler(UserDbContext context) : ICommandHandler<UpdateRoleGroupCommand, UpdateRoleGroupResult>
{
  public async Task<UpdateRoleGroupResult> Handle(UpdateRoleGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.RoleGroup.Translates.Count == 0)
    {
      throw new RoleGroupBadRequestException("At least one translation is required.");
    }

    var roleGroup = await context.RoleGroups
        .Include(rg => rg.RoleGroupTranslatates)
        .FirstOrDefaultAsync(rg => rg.Id == request.Id && !rg.IsDeleted, cancellationToken);

    if (roleGroup == null)
    {
      throw new RoleGroupNotFoundException($"RoleGroup with ID '{request.Id}' not found.", request.Id);
    }

    var validTranslations = request.RoleGroup.Translates.Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue).ToList();

    //Sadece geçerli çevirilerin isimlerini kontrol et (mevcut role group hariç)
    var existingRoleGroupNames = validTranslations.Select(t => t.Name).ToList();

    var existingRoleGroups = await context.RoleGroups
        .Where(rg => rg.Id != request.Id && rg.RoleGroupTranslatates.Any(rt => existingRoleGroupNames.Contains(rt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingRoleGroups.Any())
    {
      throw new RoleGroupBadRequestException("A role group with one of the provided names already exists.");
    }

    // Mevcut çevirileri veritabanından sil
    context.RoleGroupTranslatates.RemoveRange(roleGroup.RoleGroupTranslatates);

    // Yeni çevirileri ekle
    foreach (var transDto in validTranslations)
    {
      var newTranslate = RoleGroupTranslatate.Create(transDto.Name, transDto.Description, transDto.LanguageId, roleGroup.Id);
      context.RoleGroupTranslatates.Add(newTranslate);
    }

    await context.SaveChangesAsync(cancellationToken);

    return new UpdateRoleGroupResult(roleGroup.Id);
  }
}