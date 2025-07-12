using WinApps.Modules.Users.Users.Branches.DomainExtensions;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.QueryParams;

namespace WinApps.Modules.Users.Users.Branches.Features.GetTenantBranches;

public record GetTenantBranchesQuery(BranchParams Parameters, Guid TenantId) : IQuery<GetTenantBranchesResult>, ICachableRequest, IAuthorizeRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantBranches, Parameters, TenantId);
  public string CacheGroupKey => CacheKeys.TenantBranches;
  public TimeSpan? CacheExpiration => null;
  public List<string> PermissionRoles => [RoleNames.BranchRead];
}

public record GetTenantBranchesResult(IEnumerable<BranchDto> Branches, PaginationMetaData MetaData);

public class GetTenantBranchesHandler : IQueryHandler<GetTenantBranchesQuery, GetTenantBranchesResult>
{
  private readonly UserDbContext _context;

  public GetTenantBranchesHandler(UserDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantBranchesResult> Handle(GetTenantBranchesQuery request, CancellationToken cancellationToken)
  {
    var query = _context.Branches
        .Include(x => x.AppTenant)
        .AsNoTracking()
        .ApplyBranchFilters(request.Parameters)
        .ApplyBranchOrdering()
        .ProjectBranchListToDto();

    var branches = await PagedList<BranchDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetTenantBranchesResult(branches, branches.MetaData);
  }
}