using Accounting.ExpensePens.DomainExtensions;
using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Exceptions;

namespace Accounting.ExpensePens.Features.TenantExpensePens.GetTenantExpensePenById;

public record GetTenantExpensePenByIdQuery(Guid Id, Guid? TenantId) : IQuery<GetTenantExpensePenByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantExpensePens, Id, TenantId!);
  public string CacheGroupKey => CacheKeys.TenantExpensePens;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantExpensePenByIdResult(TenantExpensePenDto TenantExpensePen);

public class GetTenantExpensePenByIdHandler : IQueryHandler<GetTenantExpensePenByIdQuery, GetTenantExpensePenByIdResult>
{
  private readonly AccountingDbContext _context;

  public GetTenantExpensePenByIdHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetTenantExpensePenByIdResult> Handle(GetTenantExpensePenByIdQuery request, CancellationToken cancellationToken)
  {
    var tenantExpensePen = await _context.TenantExpensePens
        .Include(tep => tep.TenantExpensePenTranslates)
            .ThenInclude(t => t.Language) 
        .AsNoTracking()
        .FirstOrDefaultAsync(tep => tep.Id == request.Id, cancellationToken);

    if (tenantExpensePen == null)
    {
      throw new ExpensePenNotFoundException(request.Id);
    }

    var tenantExpensePenDto = tenantExpensePen.ProjectTenantExpensePenToDto();

    return new GetTenantExpensePenByIdResult(tenantExpensePenDto);
  }
}