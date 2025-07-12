using WinApps.Modules.Users.Users.Branches.DomainExtensions;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Features.GetTenantBranchById;

public record GetTenantBranchByIdQuery(Guid Id, Guid TenantId) : IQuery<GetTenantBranchByIdResult>, ICachableRequest,IAuthorizeRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Branches, Id, TenantId);
  public string CacheGroupKey => CacheKeys.Branches;
  public TimeSpan? CacheExpiration => null;
  public List<string> PermissionRoles => [RoleNames.BranchRead];
}

public record GetTenantBranchByIdResult(BranchDto Branch);

public class GetTenantBranchByIdHandler : IQueryHandler<GetTenantBranchByIdQuery, GetTenantBranchByIdResult>
{
  private readonly UserDbContext _context;

  public GetTenantBranchByIdHandler(UserDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantBranchByIdResult> Handle(GetTenantBranchByIdQuery request, CancellationToken cancellationToken)
  {
    var branch = await _context.Branches
        .Include(x => x.AppTenant)
        .IgnoreQueryFilters()
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == request.TenantId && !x.IsDeleted, cancellationToken);

    if (branch == null)
    {
      throw new BranchNotFoundException($"Branch with ID '{request.Id}' not found in this tenant.", request.Id);
    }

    var branchDto = branch.ProjectBranchToDto();

    return new GetTenantBranchByIdResult(branchDto);
  }
}