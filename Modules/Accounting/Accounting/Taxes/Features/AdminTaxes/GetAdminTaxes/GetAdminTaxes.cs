using Accounting.Taxes.DomainExtensions;
using Accounting.Taxes.Dtos;

namespace Accounting.Taxes.Features.AdminTaxes.GetAdminTaxes;

public record GetAdminTaxesQuery(GetAdminTaxesRequest Parameters) : IQuery<GetAdminTaxesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminTaxes, Parameters);
  public string CacheGroupKey => CacheKeys.AdminTaxes;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminTaxesResult(IEnumerable<TaxDto> Taxes, PaginationMetaData MetaData);

public class GetAdminTaxesHandler : IQueryHandler<GetAdminTaxesQuery, GetAdminTaxesResult>
{
  private readonly AccountingDbContext _context;

  public GetAdminTaxesHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminTaxesResult> Handle(GetAdminTaxesQuery request, CancellationToken cancellationToken)
  {
    var query = _context.Taxes
        .Include(t => t.TaxTranslates)
            .ThenInclude(tt => tt.Language)
        .AsNoTracking()
        .ApplyTaxFilters(request.Parameters)
        .ApplyTaxOrdering()
        .ProjectTaxListToDto();

    var taxes = await PagedList<TaxDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetAdminTaxesResult(taxes, taxes.MetaData);
  }
}