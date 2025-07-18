using Accounting.TaxGroups.DomainExtensions;
using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Exceptions;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.GetTenantTaxGroupById;

public record GetTenantTaxGroupByIdQuery(Guid Id,Guid? tenantId) : IQuery<GetTenantTaxGroupByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantTaxGroups, Id,tenantId!);
  public string CacheGroupKey => CacheKeys.TenantTaxGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantTaxGroupByIdResult(TenantTaxGroupDto TenantTaxGroup);

public class GetTenantTaxGroupByIdHandler : IQueryHandler<GetTenantTaxGroupByIdQuery, GetTenantTaxGroupByIdResult>
{
  private readonly AccountingDbContext _context;

  public GetTenantTaxGroupByIdHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantTaxGroupByIdResult> Handle(GetTenantTaxGroupByIdQuery request, CancellationToken cancellationToken)
  {
    var tenantTaxGroup = await _context.TenantTaxGroups
        .Include(ttg => ttg.TenantTaxGroupTranslates)
        .ThenInclude(t => t.Language)
        .AsNoTracking()
        .FirstOrDefaultAsync(ttg => ttg.Id == request.Id, cancellationToken);

    if (tenantTaxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.Id);
    }

    var tenantTaxGroupDto = tenantTaxGroup.ProjectTenantTaxGroupToDto();

    return new GetTenantTaxGroupByIdResult(tenantTaxGroupDto);
  }
}