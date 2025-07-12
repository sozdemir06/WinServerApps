using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Models;
using WinApps.Modules.Users.Users.Branches.QueryParams;

namespace WinApps.Modules.Users.Users.Branches.DomainExtensions;

public static class BranchExtensions
{
  public static IQueryable<Branch> ApplyBranchFilters(
      this IQueryable<Branch> query,
      BranchParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(b =>
          b.Name.ToLower().Contains(searchTerm) ||
          b.Code.ToLower().Contains(searchTerm) ||
          (b.Description != null && b.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(b => b.IsActive == parameters.IsActive.Value);
    }

    return query;
  }

  public static IQueryable<Branch> ApplyBranchOrdering(
      this IQueryable<Branch> query)
  {
    return query.OrderByDescending(b => b.CreatedAt);
  }

  public static BranchDto ProjectBranchToDto(this Branch branch) =>
      new(
          branch.Id,
          branch.Name,
          branch.Code,
          branch.Address,
          branch.Phone,
          branch.Email,
          branch.IsActive,
          branch.Description,
          branch.TenantId,
          branch.CreatedBy,
          branch.ModifiedBy,
          branch.CreatedAt,
          branch.UpdatedAt,
          branch.AppTenant.ProjectAppTenantToDto());

  public static IQueryable<BranchDto> ProjectBranchListToDto(this IQueryable<Branch> branches) =>
    branches.Select(b => new BranchDto(
        b.Id,
        b.Name,
        b.Code,
        b.Address,
        b.Phone,
        b.Email,
        b.IsActive,
        b.Description,
        b.TenantId,
        b.CreatedBy,
        b.ModifiedBy,
        b.CreatedAt,
        b.UpdatedAt,
        b.AppTenant.ProjectAppTenantToDto()));
}