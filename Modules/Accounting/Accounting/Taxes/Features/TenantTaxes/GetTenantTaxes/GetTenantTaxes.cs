using Accounting.Taxes.DomainExtensions;
using Accounting.Taxes.Dtos;

namespace Accounting.Taxes.Features.TenantTaxes.GetTenantTaxes;

public record GetTenantTaxesQuery(GetTenantTaxesRequest Parameters,Guid? tenantId) : IQuery<GetTenantTaxesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantTaxes, Parameters,tenantId!);
  public string CacheGroupKey => CacheKeys.TenantTaxes;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantTaxesResult(IEnumerable<TenantTaxDto> TenantTaxes, PaginationMetaData MetaData);

public class GetTenantTaxesHandler : IQueryHandler<GetTenantTaxesQuery, GetTenantTaxesResult>
{
  private readonly AccountingDbContext _context;

  public GetTenantTaxesHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantTaxesResult> Handle(GetTenantTaxesQuery request, CancellationToken cancellationToken)
  {
    var query = _context.TenantTaxes
        .Include(tt => tt.TenantTaxTranslates)
            .ThenInclude(ttt => ttt.Language)
        .AsNoTracking()
        .ApplyTenantTaxFilters(request.Parameters)
        .ApplyTenantTaxOrdering()
        .ProjectTenantTaxListToDto();

    var tenantTaxes = await PagedList<TenantTaxDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetTenantTaxesResult(tenantTaxes, tenantTaxes.MetaData);
  }
}