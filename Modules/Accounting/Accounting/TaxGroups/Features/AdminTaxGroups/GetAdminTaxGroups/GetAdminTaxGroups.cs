using Accounting.TaxGroups.DomainExtensions;
using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.QueryParams;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.GetAdminTaxGroups;

public record GetAdminTaxGroupsQuery(GetAdminTaxGroupsRequest Parameters) : IQuery<GetAdminTaxGroupsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminTaxGroups, Parameters);
  public string CacheGroupKey => CacheKeys.AdminTaxGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminTaxGroupsResult(IEnumerable<TaxGroupDto> TaxGroups, PaginationMetaData MetaData);

public class GetAdminTaxGroupsHandler : IQueryHandler<GetAdminTaxGroupsQuery, GetAdminTaxGroupsResult>
{
  private readonly AccountingDbContext _context;

  public GetAdminTaxGroupsHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminTaxGroupsResult> Handle(GetAdminTaxGroupsQuery request, CancellationToken cancellationToken)
  {
    var query = _context.TaxGroups
        .Include(tg => tg.TaxGroupTranslates)
            .ThenInclude(t => t.Language)
        .Include(tg => tg.Taxes)
            .ThenInclude(t => t.TaxTranslates)
            .ThenInclude(t => t.Language)
        .AsNoTracking()
        .ApplyTaxGroupFilters(request.Parameters)
        .ApplyTaxGroupOrdering()
        .ProjectTaxGroupListToDto();

    var taxGroups = await PagedList<TaxGroupDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetAdminTaxGroupsResult(taxGroups, taxGroups.MetaData);
  }
}