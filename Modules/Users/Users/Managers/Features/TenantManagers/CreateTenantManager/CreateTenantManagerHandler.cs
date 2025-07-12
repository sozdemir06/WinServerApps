using Shared.Services.Securities;
using Users.Managers.Dtos;
using Users.Managers.Exceptions;
using Users.Managers.Models;

namespace Users.Managers.Features.TenantManagers.CreateTenantManager;

public record CreateTenantManagerCommand(ManagerDto Manager, Guid TenantId) : ICommand<CreateTenantManagerResult>, ICacheRemovingRequest,IAuthorizeRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantManagers];

  public List<string> PermissionRoles => [RoleNames.TenantManagerEdit];
}

public record CreateTenantManagerResult(Guid Id);

public class CreateTenantManagerCommandValidator : AbstractValidator<CreateTenantManagerCommand>
{
  public CreateTenantManagerCommandValidator()
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

    RuleFor(x => x.Manager.Password)
        .NotEmpty()
        .WithMessage("Password is required")
        .MinimumLength(6)
        .WithMessage("Password must be at least 6 characters")
        .MaximumLength(50)
        .WithMessage("Password cannot exceed 50 characters");
  }
}

public class CreateTenantManagerHandler(
    UserDbContext dbContext,
    ILocalizationService localizationService,
    ILogger<CreateTenantManagerHandler> logger) : ICommandHandler<CreateTenantManagerCommand, CreateTenantManagerResult>
{
  public async Task<CreateTenantManagerResult> Handle(CreateTenantManagerCommand request, CancellationToken cancellationToken)
  {
    // Check if username exists
    var existingManager = await dbContext.Managers
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(x => x.UserName == request.Manager.UserName.ToUpperInvariant(), cancellationToken);

    if (existingManager != null)
    {
      throw new ManagerAlreadyExistsException("username", request.Manager.UserName);
    }

    // Check if email exists
    existingManager = await dbContext.Managers
        .FirstOrDefaultAsync(x => x.Email == request.Manager.Email.ToUpperInvariant(), cancellationToken);

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
      // Create password hash
      byte[] passwordHash, passwordSalt;
      HashingHelper.CreatePaswordHash(request.Manager.Password!, out passwordHash, out passwordSalt);

      var manager = Manager.Create(
          request.Manager.UserName,
          request.Manager.Email,
          request.Manager.PhoneNumber ?? string.Empty,
          request.Manager.FirstName,
          request.Manager.LastName,
          request.Manager.PhotoUrl ?? string.Empty,
          request.Manager.UserName.ToUpperInvariant(),
          request.Manager.Email.ToUpperInvariant(),
          false, // Tenant managers are not admin
          true,  // Tenant managers are managers
          passwordHash,
          passwordSalt,
          request.TenantId,
          request.Manager.BranchId,
          request.Manager.IsActive
      );

      await dbContext.Managers.AddAsync(manager, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return new CreateTenantManagerResult(manager.Id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to create tenant manager. Username: {Username}, Email: {Email}",
          request.Manager.UserName, request.Manager.Email);

      await transaction.RollbackAsync(cancellationToken);
      throw new ManagerValidationException(await localizationService.Translate("FailedToCreateTenantManager"));
    }
  }
}