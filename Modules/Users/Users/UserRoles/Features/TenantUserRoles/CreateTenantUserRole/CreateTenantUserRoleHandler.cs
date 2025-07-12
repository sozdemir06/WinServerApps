using Users.UserRoles.Constants;
using Users.UserRoles.Dtos;
using Users.UserRoles.Exceptions;
using Users.UserRoles.Models;

namespace Users.UserRoles.Features.TenantUserRoles.CreateTenantUserRole;

public record CreateTenantUserRoleCommand(UserRoleDto UserRole) : ICommand<CreateTenantUserRoleResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.UserRoles];
}

public record CreateTenantUserRoleResult(Guid Id);

public class CreateTenantUserRoleCommandValidator : AbstractValidator<CreateTenantUserRoleCommand>
{
  public CreateTenantUserRoleCommandValidator()
  {
    RuleFor(x => x.UserRole.ManagerId)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.UserIdRequired);

    RuleFor(x => x.UserRole.RoleId)
        .NotEmpty()
        .WithMessage(UserRoleConstants.Validation.RoleIdRequired);
  }
}

public class CreateTenantUserRoleHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<CreateTenantUserRoleHandler> logger) : ICommandHandler<CreateTenantUserRoleCommand, CreateTenantUserRoleResult>
{
  public async Task<CreateTenantUserRoleResult> Handle(CreateTenantUserRoleCommand request, CancellationToken cancellationToken)
  {
    // Verify manager exists and is a tenant manager
    var manager = await dbContext.Managers
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.UserRole.ManagerId, cancellationToken);

    if (manager == null)
    {
      throw new UserRoleValidationException(await localizationService.Translate("ManagerNotFound"));
    }

    if (!manager.IsActive)
    {
      throw new UserRoleValidationException(await localizationService.Translate("ManagerNotActive"));
    }

    // Verify role exists
    var role = await dbContext.AppRoles
        .FirstOrDefaultAsync(x => x.Id == request.UserRole.RoleId, cancellationToken);

    if (role == null)
    {
      throw new UserRoleValidationException(await localizationService.Translate("RoleNotFound"));
    }

    // Check if user role already exists
    var existingUserRole = await dbContext.UserRoles
        .FirstOrDefaultAsync(x => x.ManagerId == request.UserRole.ManagerId && x.RoleId == request.UserRole.RoleId, cancellationToken);

    if (existingUserRole != null)
    {
      throw new UserRoleAlreadyExistsException(UserRoleConstants.Operations.UserRoleAlreadyExists);
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      var userRole = UserRole.Create(
          request.UserRole.ManagerId,
          request.UserRole.RoleId
      );

      await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new CreateTenantUserRoleResult(userRole.Id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to create tenant user role. ManagerId: {ManagerId}, RoleId: {RoleId}",
          request.UserRole.ManagerId, request.UserRole.RoleId);

      await transaction.RollbackAsync(cancellationToken);
      throw new UserRoleValidationException(await localizationService.Translate("FailedToCreateUserRole"));
    }
  }
}