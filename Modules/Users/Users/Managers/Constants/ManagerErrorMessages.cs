namespace Users.Managers.Constants;

public static class ManagerErrorMessages
{
  public static class Operations
  {
    public const string ManagerNotFound = "Manager with ID {0} was not found.";
    public const string ManagerNotFoundByEmail = "Manager with email {0} was not found.";
    public const string ManagerNotFoundByUserName = "Manager with username {0} was not found.";
    public const string ManagerAlreadyExists = "A manager with the same {0} already exists.";
    public const string ManagerCreateFailed = "Failed to create manager.";
    public const string ManagerUpdateFailed = "Failed to update manager.";
    public const string ManagerDeleteFailed = "Failed to delete manager.";
    public const string ManagerInvalidId = "Invalid manager ID provided.";
    public const string ManagerActivationFailed = "Failed to activate manager.";
    public const string ManagerDeactivationFailed = "Failed to deactivate manager.";
    public const string ManagerPasswordChangeFailed = "Failed to change manager password.";
  }

  public static class Validation
  {
    public const string UserNameRequired = "Username is required.";
    public const string UserNameLength = "Username must be between {0} and {1} characters.";
    public const string UserNameFormat = "Username can only contain letters, numbers, and underscores.";
    public const string EmailRequired = "Email is required.";
    public const string EmailFormat = "Email must be a valid email address.";
    public const string PhoneNumberFormat = "Phone number must be in a valid format.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordLength = "Password must be between {0} and {1} characters.";
    public const string PasswordComplexity = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    public const string FirstNameRequired = "First name is required.";
    public const string FirstNameLength = "First name must be between {0} and {1} characters.";
    public const string LastNameRequired = "Last name is required.";
    public const string LastNameLength = "Last name must be between {0} and {1} characters.";
    public const string PhotoUrlFormat = "Photo URL must be a valid URL.";
    public const string TenantIdRequired = "Tenant ID is required.";
    public const string TenantNotFound = "Tenant with ID {0} was not found.";
  }

  public static class Business
  {
    public const string ManagerAlreadyActive = "Manager is already active.";
    public const string ManagerAlreadyInactive = "Manager is already inactive.";
    public const string ManagerAlreadyDeleted = "Manager is already deleted.";
    public const string ManagerNotActive = "Manager is not active.";
    public const string ManagerDeleted = "Manager has been deleted.";
    public const string InvalidCredentials = "Invalid username or password.";
    public const string AccountLocked = "Account is locked. Please try again later.";
    public const string EmailNotConfirmed = "Email is not confirmed.";
    public const string PasswordExpired = "Password has expired. Please reset your password.";
    public const string TooManyFailedAttempts = "Too many failed login attempts. Account has been locked.";
    public const string TenantInactive = "Tenant is inactive. Please contact your administrator.";
    public const string TenantSubscriptionExpired = "Tenant subscription has expired. Please contact your administrator.";
  }
}