using Accounting.Taxes.DomainExtensions;
using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;

namespace Accounting.Taxes.Features.AdminTaxes.GetAdminTaxById;

public record GetAdminTaxByIdQuery(Guid Id) : IQuery<GetAdminTaxByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminTaxes, Id);
  public string CacheGroupKey => CacheKeys.AdminTaxes;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminTaxByIdResult(TaxDto Tax);

public class GetAdminTaxByIdHandler : IQueryHandler<GetAdminTaxByIdQuery, GetAdminTaxByIdResult>
{
  private readonly AccountingDbContext _context;

  public GetAdminTaxByIdHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminTaxByIdResult> Handle(GetAdminTaxByIdQuery request, CancellationToken cancellationToken)
  {
    var tax = await _context.Taxes
        .Include(t => t.TaxTranslates)
        .ThenInclude(tt => tt.Language)
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

    if (tax == null)
    {
      throw new TaxNotFoundException(request.Id);
    }

    var taxDto = tax.ProjectTaxToDto();

    return new GetAdminTaxByIdResult(taxDto);
  }
}