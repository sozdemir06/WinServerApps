using Users.Managers.Dtos;
using Users.Managers.Exceptions;

namespace Users.Managers.Features.AdminManagers.UpdateManager;

public record UpdateManagerCommand(Guid Id, ManagerDto Manager) : ICommand<UpdateManagerResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => ["Managers"];
}

public record UpdateManagerResult(bool Success);

public class UpdateManagerCommandValidator : AbstractValidator<UpdateManagerCommand>
{
  public UpdateManagerCommandValidator()
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

public class UpdateManagerHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<UpdateManagerHandler> logger) : ICommandHandler<UpdateManagerCommand, UpdateManagerResult>
{
  public async Task<UpdateManagerResult> Handle(UpdateManagerCommand request, CancellationToken cancellationToken)
  {
    var manager = await dbContext.Managers
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (manager == null)
    {
      throw new ManagerNotFoundException(request.Id);
    }

    // Check if username exists for another manager
    var existingManager = await dbContext.Managers
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(x => x.UserName == request.Manager.UserName && x.Id != request.Id, cancellationToken);

    if (existingManager != null)
    {
      throw new ManagerAlreadyExistsException("username", request.Manager.UserName);
    }

    // Check if email exists for another manager
    existingManager = await dbContext.Managers
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(x => x.Email == request.Manager.Email.ToUpperInvariant() && x.Id != request.Id, cancellationToken);

    if (existingManager != null)
    {
      throw new ManagerAlreadyExistsException("email", request.Manager.Email);
    }

    // Verify tenant exists if provided
    if (request.Manager.TenantId.HasValue)
    {
      var tenantExists = await dbContext.AppTenants
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(x => x.Id == request.Manager.TenantId, cancellationToken);

      if (tenantExists == null)
      {
        throw new ManagerValidationException(await localizationService.Translate("TenantNotFound"));
      }
    }

    // Verify branch exists if provided
    if (request.Manager.BranchId.HasValue)
    {
      var branchExists = await dbContext.Branches
          .IgnoreQueryFilters()
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
          request.Manager.IsAdmin,
          request.Manager.TenantId,
          request.Manager.BranchId
      );

      if (request.Manager.IsActive)
        manager.Activate();
      else
        manager.Deactivate();

      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new UpdateManagerResult(true);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to update manager. Id: {Id}, Username: {Username}, Email: {Email}",
          request.Id, request.Manager.UserName, request.Manager.Email);

      await transaction.RollbackAsync(cancellationToken);
      throw new ManagerValidationException(await localizationService.Translate("FailedToUpdateManager"));
    }
  }
}