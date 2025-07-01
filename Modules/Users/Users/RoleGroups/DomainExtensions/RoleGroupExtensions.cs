using Users.AppRoles.Dtos;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.models;
using Users.RoleGroups.QueryParams;

namespace Users.RoleGroups.DomainExtensions;

public static class RoleGroupExtensions
{
  public static IQueryable<RoleGroup> ApplyRoleGroupFilters(
      this IQueryable<RoleGroup> query,
      RoleGroupParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(rg =>
          rg.RoleGroupTranslatates.Any(rt =>
              rt.Name.ToLower().Contains(searchTerm) ||
              (rt.Description != null && rt.Description.ToLower().Contains(searchTerm))));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(rg => rg.IsActive == parameters.IsActive.Value);
    }

    if (parameters.LanguageId.HasValue)
    {
      query = query.Where(rg => rg.RoleGroupTranslatates.Any(rt => rt.LanguageId == parameters.LanguageId.Value));
    }

    return query;
  }

  public static IQueryable<RoleGroup> ApplyRoleGroupOrdering(
      this IQueryable<RoleGroup> query)
  {
    return query.OrderByDescending(rg => rg.CreatedAt);
  }

  public static RoleGroupDto ProjectRoleGroupToDto(this RoleGroup roleGroup) =>
      new(
        roleGroup.Id,
        roleGroup.ViewPermission,
        [.. roleGroup.RoleGroupTranslatates.Select(rt => new RoleGroupTranslationDto(
              rt.LanguageId ?? Guid.Empty,
              rt.Name,
              rt.Description,
              rt.Language?.Code ?? string.Empty
            ))],
        [.. roleGroup.RoleGroupItems.Select(ri => new RoleGroupItemDto(
              ri.AppRoleId,
              new AppRoleDto(ri.AppRole!.Id, ri.AppRole!.Name, ri.AppRole!.NormalizedName, ri.AppRole!.Description, ri.AppRole!.RoleLanguageKey, ri.AppRole!.IsActive)
            ))]
      );

  public static IQueryable<RoleGroupDto> ProjectRoleGroupListToDto(this IQueryable<RoleGroup> roleGroups) =>
      roleGroups.Select(rg => new RoleGroupDto(
        rg.Id,
        rg.ViewPermission,
        rg.RoleGroupTranslatates.Select(rt => new RoleGroupTranslationDto(
              rt.LanguageId ?? Guid.Empty,
              rt.Name,
              rt.Description,
              rt.Language!.Code
            ))
            .ToList(),
        rg.RoleGroupItems.Select(ri => new RoleGroupItemDto(
              ri.AppRoleId,
              new AppRoleDto(ri.AppRole!.Id, ri.AppRole!.Name, ri.AppRole!.NormalizedName, ri.AppRole!.Description, ri.AppRole!.RoleLanguageKey, ri.AppRole!.IsActive)
            ))
            .ToList()
      ));
}