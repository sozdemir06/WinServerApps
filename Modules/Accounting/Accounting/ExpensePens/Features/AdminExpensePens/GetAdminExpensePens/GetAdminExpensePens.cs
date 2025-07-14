using Accounting.ExpensePens.DomainExtensions;
using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.QueryParams;

namespace Accounting.ExpensePens.Features.AdminExpensePens.GetAdminExpensePens;

public record GetAdminExpensePensQuery(GetAdminExpensePensRequest Parameters) : IQuery<GetAdminExpensePensResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminExpensePens, Parameters);
  public string CacheGroupKey => CacheKeys.AdminExpensePens;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminExpensePensResult(IEnumerable<ExpensePenDto> ExpensePens, PaginationMetaData MetaData);

public class GetAdminExpensePensHandler : IQueryHandler<GetAdminExpensePensQuery, GetAdminExpensePensResult>
{
  private readonly AccountingDbContext _context;

  public GetAdminExpensePensHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminExpensePensResult> Handle(GetAdminExpensePensQuery request, CancellationToken cancellationToken)
  {
    var query = _context.ExpensePens
        .Include(ep => ep.ExpensePenTranslates)
            .ThenInclude(t => t.Language)
        .AsNoTracking()
        .ApplyExpensePenFilters(request.Parameters)
        .ApplyExpensePenOrdering()
        .ProjectExpensePenListToDto();

    var expensePens = await PagedList<ExpensePenDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetAdminExpensePensResult(expensePens, expensePens.MetaData);
  }
}