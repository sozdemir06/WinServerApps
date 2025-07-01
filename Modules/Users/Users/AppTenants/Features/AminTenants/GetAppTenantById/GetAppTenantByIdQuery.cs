

namespace Modules.Users.Users.AppTenants.Features.GetAppTenantById;

public record GetAppTenantByIdQuery(Guid Id) : IQuery<GetAppTenantByIdResult>;
public record GetAppTenantByIdResult(AppTenantDto AppTenant);

public class GetAppTenantByIdQueryHandler : IQueryHandler<GetAppTenantByIdQuery, GetAppTenantByIdResult>
{
  private readonly UserDbContext _dbContext;

  public GetAppTenantByIdQueryHandler(UserDbContext dbContext)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
  }

  public async Task<GetAppTenantByIdResult> Handle(GetAppTenantByIdQuery request, CancellationToken cancellationToken)
  {
    var appTenant = await _dbContext.AppTenants
      .AsNoTracking()
      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (appTenant == null)
      throw new AppTenantNotFoundException("AppTenant not found", request.Id);

    var appTenantDto = appTenant.ProjectAppTenantToDto();

    return new GetAppTenantByIdResult(appTenantDto);
  }
}