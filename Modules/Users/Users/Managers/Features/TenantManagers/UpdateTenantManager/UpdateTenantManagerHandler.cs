using Users.Managers.Dtos;
using Users.Managers.Exceptions;

namespace Users.Managers.Features.TenantManagers.UpdateTenantManager;

public record UpdateTenantManagerCommand(Guid Id, ManagerDto Manager, Guid TenantId) : ICommand<UpdateTenantManagerResult>, ICacheRemovingRequest,IAuthorizeRequest
{
  public List<string> CacheKeysToRemove => ["TenantManagers"];

  public List<string> PermissionRoles => [RoleNames.TenantManagerEdit];
}

public record UpdateTenantManagerResult(bool Success);

public class UpdateTenantManagerCommandValidator : AbstractValidator<UpdateTenantManagerCommand>
{
  public UpdateTenantManagerCommandValidator()
  {
    RuleFor(x => x.Manager.FirstName)
        .NotEmpty()
        .WithMessage("First name is required")
        .MaximumLength(50)
        .WithMessage("First name cannot exceed 50 characters");

    RuleFor(x => x.Manager.LastName)
        .NotEmpty()
        .WithMessage("Last name is required")
        .MaximumLength(50)
        .WithMessage("Last name cannot exceed 50 characters");

    RuleFor(x => x.Manager.UserName)
        .NotEmpty()
        .WithMessage("Username is required")
        .MinimumLength(3)
        .WithMessage("Username must be at least 3 characters")
        .MaximumLength(50)
        .WithMessage("Username cannot exceed 50 characters");

    RuleFor(x => x.Manager.Email)
        .NotEmpty()
        .WithMessage("Email is required")
        .EmailAddress()
        .WithMessage("Invalid email format")
        .MaximumLength(256)
        .WithMessage("Email cannot exceed 256 characters");

    RuleFor(x => x.Manager.PhoneNumber)
        .MaximumLength(20)
        .WithMessage("Phone number cannot exceed 20 characters");

    RuleFor(x => x.Manager.PhotoUrl)
        .MaximumLength(500)
        .WithMessage("Photo URL cannot exceed 500 characters");


  }
}

public class UpdateTenantManagerHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<UpdateTenantManagerHandler> logger) : ICommandHandler<UpdateTenantManagerCommand, UpdateTenantManagerResult>
{
  public async Task<UpdateTenantManagerResult> Handle(UpdateTenantManagerCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
        .Where(x => !x.IsAdmin) // Filter for tenant managers only
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.Id);
    }

    // Check if email exists for another manager
   var existingManager = await dbContext.Managers
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(x => x.NormalizedEmail == request.Manager.Email.ToUpperInvariant() && x.Id != request.Id, cancellationToken);

    if (existingManager != null)
    {
      throw new ManagerAlreadyExistsException("email", request.Manager.Email);
    }

    // Verify tenant exists
    var tenantExists = await dbContext.AppTenants
        .IgnoreQueryFilters()
        .SingleOrDefaultAsync(x => x.Id == request.TenantId, cancellationToken);

    if (tenantExists == null)
    {
      throw new ManagerValidationException(await localizationService.Translate("TenantNotFound"));
    }

    // Verify branch exists if provided
    if (request.Manager.BranchId.HasValue)
    {
      var branchExists = await dbContext.Branches
          .SingleOrDefaultAsync(x => x.Id == request.Manager.BranchId, cancellationToken);

      if (branchExists == null)
      {
        throw new ManagerValidationException(await localizationService.Translate("BranchNotFound"));
      }
    }

    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      manager.Update(
          request.Manager.FirstName,
          request.Manager.LastName,
          request.Manager.UserName,
          request.Manager.Email,
          request.Manager.PhoneNumber ?? string.Empty,
          true, // Tenant managers are always managers
          request.TenantId,
          request.Manager.BranchId
      );

      if (request.Manager.IsActive)
        manager.Activate();
      else
        manager.Deactivate();

      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateTenantManagerResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to update tenant manager. Id: {Id}, Username: {Username}, Email: {Email}",
          request.Id, request.Manager.UserName, request.Manager.Email);

      await transaction.RollbackAsync(cancellationToken);
      throw new ManagerValidationException(await localizationService.Translate("FailedToUpdateTenantManager"));
    }
  }
}