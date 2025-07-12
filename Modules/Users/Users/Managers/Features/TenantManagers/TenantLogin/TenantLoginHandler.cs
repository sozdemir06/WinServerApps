using Shared.Dtos;
using Shared.Services.Securities;
using Users.Data.Services.Securities;
using Users.Managers.Dtos;
using Users.Managers.Exceptions;
using WinApps.Modules.Users.Users.Managers.Dtos;
using WinApps.Modules.Users.Users.Managers.Exceptions;

namespace WinApps.Modules.Users.Users.Managers.Features.TenantManagers.TenantLogin;

public record TenantLoginCommand(LoginRequestDto Login) : ICommand<TenantLoginResult>;

public record TenantLoginResult(LoginResponseDto LoginResponse);

public class TenantLoginCommandValidator : AbstractValidator<TenantLoginCommand>
{
  public TenantLoginCommandValidator()
  {
    // Option 1: Basic When condition
    When(x => x.Login != null, () =>
    {
      RuleFor(x => x.Login.Email)
          .NotEmpty().WithMessage("Email address is required.")
          .EmailAddress().WithMessage("A valid email address is required.");
      // You can add other rules for x.Login.Password here too
      RuleFor(x => x.Login.Password)
          .NotEmpty().WithMessage("Password is required.");
    });
  }
}

public class TenantLoginHandler(
  ILogger<TenantLoginHandler> logger,
  UserDbContext dbContext,
  IManagerTokenService managerTokenService,
  ILocalizationService localizationService) : ICommandHandler<TenantLoginCommand, TenantLoginResult>
{
  public async Task<TenantLoginResult> Handle(TenantLoginCommand request, CancellationToken cancellationToken)
  {
    try
    {
      logger.LogInformation("Tenant login attempt for email: {Email}", request.Login.Email);

      var manager = await dbContext.Managers
          .Include(x => x.UserRoles)
          .ThenInclude(x => x.AppRole)
          .IgnoreQueryFilters()
          .AsNoTracking()
          .FirstOrDefaultAsync(m => m.Email.Trim().ToLower() == request.Login.Email!.Trim().ToLower() && !m.IsAdmin, cancellationToken);

      if (manager == null) throw new ManagerNotFoundException(request.Login.Email!);

      if (!manager.IsActive) throw new ManagerValidationException(request.Login.Email!, "Manager is not active");

      var checkPassword = HashingHelper.VerifyPasswordHash(request.Login.Password!, manager!.PasswordHash, manager.PasswordSalt);
      if (!checkPassword) throw new InvalidPasswordException(await localizationService.Translate("InvalidCurrentPassword"));

      var token = await managerTokenService.CreateTokenAsync(manager);

      // var userRoles = manager.UserRoles.Select(x => new ManagerRoleDto(
      //   x.AppRole.Id,
      //   x.AppRole.Name,
      //   x.AppRole.NormalizedName,
      //   x.AppRole.Description
      // )).ToList();
      // var roles = userRoles.Select(x => x.Name).ToList();
      var loginResponse = new LoginResponseDto
      (
        token,
        manager.FirstName + " " + manager.LastName,
        manager.Email
      );

      return new TenantLoginResult(loginResponse);

    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error during tenant login for email: {Email}", request.Login.Email);
      throw new LoginFailedException(ex.Message);
    }
  }
}