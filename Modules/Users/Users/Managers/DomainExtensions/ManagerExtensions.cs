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
      manager.CreatedAt 
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
      m.CreatedAt,
      null,
      m.Branch!.Name,
      m.Tenant!.Name,
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


    if (parameters.BranchId.HasValue)
    {
      query = query.Where(m => m.BranchId == parameters.BranchId.Value);
    }

    if (!string.IsNullOrWhiteSpace(parameters.Name))
    {
      
      query = query.Where(m => m.FirstName.ToLower().Contains(parameters.Name.ToLower()) || m.LastName.ToLower().Contains(parameters.Name.ToLower()));
    }

    return query;
  }


  public static IQueryable<Manager> ApplyQueryParams(this IQueryable<Manager> query, ManagerParams? parameters)
  {
    return query
      .ApplySearch(parameters?.Search)
      .ApplyFilters(parameters);
     
  }
}