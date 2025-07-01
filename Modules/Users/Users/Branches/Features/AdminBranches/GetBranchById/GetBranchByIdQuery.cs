using WinApps.Modules.Users.Users.Branches.DomainExtensions;
using WinApps.Modules.Users.Users.Branches.Dtos;
using WinApps.Modules.Users.Users.Branches.Exceptions;


namespace WinApps.Modules.Users.Users.Branches.Features.GetBranchById;

public record GetBranchByIdQuery(Guid Id) : IQuery<GetBranchByIdResult>, ICachableRequest
{
  public string CacheKey => $"{CacheKeys.Branches}:{Id}";
  public string CacheGroupKey => CacheKeys.Branches;
  public TimeSpan? CacheExpiration => null;
}

public record GetBranchByIdResult(BranchDto Branch);

public class GetBranchByIdQueryHandler : IQueryHandler<GetBranchByIdQuery, GetBranchByIdResult>
{
  private readonly UserDbContext _dbContext;

  public GetBranchByIdQueryHandler(UserDbContext dbContext)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
  }

  public async Task<GetBranchByIdResult> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
  {
    var branch = await _dbContext.Branches
        .IgnoreQueryFilters()
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (branch == null)
      throw new BranchNotFoundException("Branch not found", request.Id);

    var branchDto = branch.ProjectBranchToDto();

    return new GetBranchByIdResult(branchDto);
  }
}