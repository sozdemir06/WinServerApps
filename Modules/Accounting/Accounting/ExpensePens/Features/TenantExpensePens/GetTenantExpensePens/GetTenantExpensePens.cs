using Accounting.ExpensePens.DomainExtensions;
using Accounting.ExpensePens.Dtos;

namespace Accounting.ExpensePens.Features.TenantExpensePens.GetTenantExpensePens;

public record GetTenantExpensePensQuery(GetTenantExpensePensRequest Parameters, Guid? TenantId) : IQuery<GetTenantExpensePensResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantExpensePens, Parameters, TenantId!);
  public string CacheGroupKey => CacheKeys.TenantExpensePens;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantExpensePensResult(IEnumerable<TenantExpensePenDto> TenantExpensePens, PaginationMetaData MetaData);

public class GetTenantExpensePensHandler : IQueryHandler<GetTenantExpensePensQuery, GetTenantExpensePensResult>
{
  private readonly AccountingDbContext _context;

  public GetTenantExpensePensHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantExpensePensResult> Handle(GetTenantExpensePensQuery request, CancellationToken cancellationToken)
  {
    var query = _context.TenantExpensePens
        .Include(tep => tep.TenantExpensePenTranslates)
            .ThenInclude(t => t.Language)
        .AsNoTracking()
        .ApplyTenantExpensePenFilters(request.Parameters)
        .ApplyTenantExpensePenOrdering()
        .ProjectTenantExpensePenListToDto();

    var tenantExpensePens = await PagedList<TenantExpensePenDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetTenantExpensePensResult(tenantExpensePens, tenantExpensePens.MetaData);
  }
}