
namespace Users.RoleGroups.Dtos
{
    public record RoleGroupTranslationDto(Guid? LanguageId, string Name, string? Description,string LanguageCode);
}