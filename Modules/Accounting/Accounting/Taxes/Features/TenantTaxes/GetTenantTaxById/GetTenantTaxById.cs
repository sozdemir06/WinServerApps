using Accounting.Taxes.DomainExtensions;
using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;

namespace Accounting.Taxes.Features.TenantTaxes.GetTenantTaxById;

public record GetTenantTaxByIdQuery(Guid Id,Guid? tenantId) : IQuery<GetTenantTaxByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantTaxes, Id,tenantId!);
  public string CacheGroupKey => CacheKeys.TenantTaxes;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantTaxByIdResult(TenantTaxDto TenantTax);

public class GetTenantTaxByIdHandler : IQueryHandler<GetTenantTaxByIdQuery, GetTenantTaxByIdResult>
{
  private readonly AccountingDbContext _context;

  public GetTenantTaxByIdHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantTaxByIdResult> Handle(GetTenantTaxByIdQuery request, CancellationToken cancellationToken)
  {
    var tenantTax = await _context.TenantTaxes
        .Include(tt => tt.TenantTaxTranslates)
        .ThenInclude(ttt => ttt.Language)
        .AsNoTracking()
        .FirstOrDefaultAsync(tt => tt.Id == request.Id, cancellationToken);

    if (tenantTax == null)
    {
      throw new TaxNotFoundException(request.Id);
    }

    var tenantTaxDto = tenantTax.ProjectTenantTaxToDto();

    return new GetTenantTaxByIdResult(tenantTaxDto);
  }
}