using WinApps.Modules.Users.Users.Branches.DomainExtensions;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.QueryParams;

namespace WinApps.Modules.Users.Users.Branches.Features.GetBranches;

public record GetBranchesQuery(BranchParams Parameters) : IQuery<GetBranchesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Branches, Parameters);
  public string CacheGroupKey => CacheKeys.Branches;
  public TimeSpan? CacheExpiration => null;
}

public record GetBranchesResult(IEnumerable<BranchDto> Branches, PaginationMetaData MetaData);

public class GetBranchesHandler : IQueryHandler<GetBranchesQuery, GetBranchesResult>
{
  private readonly UserDbContext _context;

  public GetBranchesHandler(UserDbContext context)
  {
    _context = context;
  }

  public async Task<GetBranchesResult> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
  {
    var query = _context.Branches
        .Include(x=>x.AppTenant)
        .IgnoreQueryFilters()
        .Where(x => !x.IsDeleted)
        .AsNoTracking()
        .ApplyBranchFilters(request.Parameters)
        .ApplyBranchOrdering()
        .ProjectBranchListToDto();

    var branches = await PagedList<BranchDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetBranchesResult(branches, branches.MetaData);
  }
}