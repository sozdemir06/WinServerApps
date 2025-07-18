using Accounting.TaxGroups.DomainExtensions;
using Accounting.TaxGroups.Dtos;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.GetTenantTaxGroups;

public record GetTenantTaxGroupsQuery(GetTenantTaxGroupsRequest Parameters,Guid? tenantId) : IQuery<GetTenantTaxGroupsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantTaxGroups, Parameters,tenantId!);
  public string CacheGroupKey => CacheKeys.TenantTaxGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantTaxGroupsResult(IEnumerable<TenantTaxGroupDto> TenantTaxGroups, PaginationMetaData MetaData);

public class GetTenantTaxGroupsHandler : IQueryHandler<GetTenantTaxGroupsQuery, GetTenantTaxGroupsResult>
{
  private readonly AccountingDbContext _context;

  public GetTenantTaxGroupsHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantTaxGroupsResult> Handle(GetTenantTaxGroupsQuery request, CancellationToken cancellationToken)
  {
    var query = _context.TenantTaxGroups
        .Include(ttg => ttg.TenantTaxGroupTranslates)
            .ThenInclude(t => t.Language)
        .Include(ttg => ttg.TenantTaxes)
            .ThenInclude(t => t.TenantTaxTranslates)
            .ThenInclude(t => t.Language)
        .AsNoTracking()
        .ApplyTenantTaxGroupFilters(request.Parameters)
        .ApplyTenantTaxGroupOrdering()
        .ProjectTenantTaxGroupListToDto();

    var tenantTaxGroups = await PagedList<TenantTaxGroupDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetTenantTaxGroupsResult(tenantTaxGroups, tenantTaxGroups.MetaData);
  }
}