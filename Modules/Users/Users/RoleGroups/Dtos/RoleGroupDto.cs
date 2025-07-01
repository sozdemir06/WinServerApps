

using Users.RoleGroups.Enums;

namespace Users.RoleGroups.Dtos
{
    public record RoleGroupDto(
        Guid Id,
        RoleGroupViewPermission ViewPermission,
        List<RoleGroupTranslationDto> Translates,
        List<RoleGroupItemDto> RoleGroupItems);
}