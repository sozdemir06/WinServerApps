using Accounting.TaxGroups.DomainExtensions;
using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Exceptions;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.GetAdminTaxGroupById;

public record GetAdminTaxGroupByIdQuery(Guid Id) : IQuery<GetAdminTaxGroupByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminTaxGroups, Id);
  public string CacheGroupKey => CacheKeys.AdminTaxGroups;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminTaxGroupByIdResult(TaxGroupDto TaxGroup);

public class GetAdminTaxGroupByIdHandler : IQueryHandler<GetAdminTaxGroupByIdQuery, GetAdminTaxGroupByIdResult>
{
  private readonly AccountingDbContext _context;

  public GetAdminTaxGroupByIdHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminTaxGroupByIdResult> Handle(GetAdminTaxGroupByIdQuery request, CancellationToken cancellationToken)
  {
    var taxGroup = await _context.TaxGroups
        .Include(tg => tg.TaxGroupTranslates)
        .ThenInclude(t => t.Language)
        .AsNoTracking()
        .FirstOrDefaultAsync(tg => tg.Id == request.Id, cancellationToken);

    if (taxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.Id);
    }

    var taxGroupDto = taxGroup.ProjectTaxGroupToDto();

    return new GetAdminTaxGroupByIdResult(taxGroupDto);
  }
}