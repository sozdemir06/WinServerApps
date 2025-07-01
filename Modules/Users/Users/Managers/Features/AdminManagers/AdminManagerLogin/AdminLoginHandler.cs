using Shared.Services.Securities;
using Users.Data.Services.Securities;
using Users.Managers.Dtos;
using Users.Managers.Exceptions;
using WinApps.Modules.Users.Users.Managers.Dtos;
using WinApps.Modules.Users.Users.Managers.Exceptions;

namespace WinApps.Modules.Users.Users.Managers.Features.AdminManagers.AdminManagerLogin;

public record AdminLoginCommand(LoginRequestDto Login) : ICommand<AdminLoginResult>;

public record AdminLoginResult(LoginResponseDto LoginResponse);

public class AdminLoginCommandValidator : AbstractValidator<AdminLoginCommand>
{
  public AdminLoginCommandValidator()
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

public class AdminLoginHandler(ILogger<AdminLoginHandler> logger, UserDbContext dbContext, IManagerTokenService managerTokenService) : ICommandHandler<AdminLoginCommand, AdminLoginResult>
{
  public async Task<AdminLoginResult> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
  {
    try
    {
      logger.LogInformation("Admin login attempt for email: {Email}", request.Login.Email);

      var normalizedEmail = request.Login.Email?.Trim().ToLower();
      var manager = await dbContext.Managers
          .IgnoreQueryFilters()
          .FirstOrDefaultAsync(m => m.Email.ToLower() == normalizedEmail && m.IsAdmin, cancellationToken);

      var checkPassword = HashingHelper.VerifyPasswordHash(request.Login.Password!, manager!.PasswordHash, manager.PasswordSalt);
      if (!checkPassword) throw new InvalidPasswordException();

      var token = await managerTokenService.CreateTokenAsync(manager);

      var loginResponse = new LoginResponseDto
      (
        token,
        manager.FirstName + " " + manager.LastName,
        manager.Email
      );

      return new AdminLoginResult(loginResponse);

    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error during admin login for email: {Email}", request.Login.Email);
      throw new LoginFailedException(ex.Message);
    }
  }
}
