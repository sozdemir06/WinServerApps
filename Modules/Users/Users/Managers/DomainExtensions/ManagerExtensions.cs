using Users.Managers.Dtos;
using Users.Managers.Models;
using Users.Managers.QueryParams;
using Users.UserRoles.Dtos;

namespace Users.Managers.DomainExtensions;

public static class ManagerExtensions
{
  public static ManagerDto ToDto(this Manager manager)
  {
    return new ManagerDto(
      manager.Id,
      manager.FirstName,
      manager.LastName,
      manager.UserName,
      manager.Email,
      manager.PhoneNumber,
      manager.PhotoUrl,
      manager.IsAdmin,
      manager.IsManager,
      manager.IsActive,
      manager.TenantId,
      manager.BranchId,
      null
    );
  }

  public static IQueryable<ManagerDto> ToDto(this IQueryable<Manager> query)
  {
    return query.Select(m => new ManagerDto(
      m.Id,
      m.FirstName,
      m.LastName,
      m.UserName,
      m.Email,
      m.PhoneNumber,
      m.PhotoUrl,
      m.IsAdmin,
      m.IsManager,
      m.IsActive,
      m.TenantId,
      m.BranchId,
      null,
      m.Branch!.Name,
      m.UserRoles.Select(x => new UserRoleDto(x.Id, x.ManagerId, x.RoleId, x.IsActive)).ToList()
    ));
  }

  public static ManagerDetailDto ToDetailDto(this Manager manager)
  {
    return new ManagerDetailDto(
      manager.Id,
      manager.FirstName,
      manager.LastName,
      manager.Email,
      manager.PhoneNumber!,
      manager.UserName,
      manager.IsAdmin,
      manager.IsManager,
      manager.IsActive,
      manager.Tenant!.Name,
      manager.Branch!.Name
    );
  }

  public static IQueryable<Manager> ApplySearch(this IQueryable<Manager> query, string? searchTerm)
  {
    if (string.IsNullOrWhiteSpace(searchTerm))
      return query;

    var search = searchTerm.ToUpperInvariant();
    return query.Where(m =>
      m.FirstName.ToUpper().Contains(search) ||
      m.LastName.ToUpper().Contains(search) ||
      m.UserName.ToUpper().Contains(search) ||
      m.Email.ToUpper().Contains(search)
    );
  }

  public static IQueryable<Manager> ApplyFilters(this IQueryable<Manager> query, ManagerParams? parameters)
  {
    if (parameters == null)
      return query;

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(m => m.IsActive == parameters.IsActive.Value);
    }

    if (parameters.IsAdmin.HasValue)
    {
      query = query.Where(m => m.IsAdmin == parameters.IsAdmin.Value);
    }

    if (parameters.IsManager.HasValue)
    {
      query = query.Where(m => m.IsManager == parameters.IsManager.Value);
    }

    if (parameters.TenantId.HasValue)
    {
      query = query.Where(m => m.TenantId == parameters.TenantId.Value);
    }

    if (parameters.BranchId.HasValue)
    {
      query = query.Where(m => m.BranchId == parameters.BranchId.Value);
    }

    return query;
  }

  public static IQueryable<Manager> ApplySorting(this IQueryable<Manager> query, ManagerParams? parameters)
  {
    if (parameters?.SortBy == null)
    {
      return parameters?.IsDescending == true
        ? query.OrderByDescending(m => m.CreatedAt)
        : query.OrderBy(m => m.CreatedAt);
    }

    return parameters.SortBy.ToLowerInvariant() switch
    {
      "firstname" => parameters.IsDescending
        ? query.OrderByDescending(m => m.FirstName)
        : query.OrderBy(m => m.FirstName),
      "lastname" => parameters.IsDescending
        ? query.OrderByDescending(m => m.LastName)
        : query.OrderBy(m => m.LastName),
      "username" => parameters.IsDescending
        ? query.OrderByDescending(m => m.UserName)
        : query.OrderBy(m => m.UserName),
      "email" => parameters.IsDescending
        ? query.OrderByDescending(m => m.Email)
        : query.OrderBy(m => m.Email),
      "createdat" => parameters.IsDescending
        ? query.OrderByDescending(m => m.CreatedAt)
        : query.OrderBy(m => m.CreatedAt),
      _ => parameters.IsDescending
        ? query.OrderByDescending(m => m.CreatedAt)
        : query.OrderBy(m => m.CreatedAt)
    };
  }

  public static IQueryable<Manager> ApplyQueryParams(this IQueryable<Manager> query, ManagerParams? parameters)
  {
    return query
      .ApplySearch(parameters?.Search)
      .ApplyFilters(parameters)
      .ApplySorting(parameters);
  }
}