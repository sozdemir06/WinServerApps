using Accounting.ExpensePens.DomainExtensions;
using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Exceptions;


namespace Accounting.ExpensePens.Features.AdminExpensePens.GetAdminExpensePenById;

public record GetAdminExpensePenByIdQuery(Guid Id) : IQuery<GetAdminExpensePenByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminExpensePens, Id);
  public string CacheGroupKey => CacheKeys.AdminExpensePens;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminExpensePenByIdResult(ExpensePenDto ExpensePen);

public class GetAdminExpensePenByIdHandler : IQueryHandler<GetAdminExpensePenByIdQuery, GetAdminExpensePenByIdResult>
{
  private readonly AccountingDbContext _context;

  public GetAdminExpensePenByIdHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminExpensePenByIdResult> Handle(GetAdminExpensePenByIdQuery request, CancellationToken cancellationToken)
  {
    var expensePen = await _context.ExpensePens
        .Include(ep => ep.ExpensePenTranslates)
            .ThenInclude(t => t.Language)
        .IgnoreQueryFilters()
        .Where(x => !x.IsDeleted)
        .AsNoTracking()
        .FirstOrDefaultAsync(ep => ep.Id == request.Id, cancellationToken);

    if (expensePen == null)
    {
      throw new ExpensePenNotFoundException(request.Id);
    }

    var expensePenDto = expensePen.ProjectExpensePenToDto();

    return new GetAdminExpensePenByIdResult(expensePenDto);
  }
}