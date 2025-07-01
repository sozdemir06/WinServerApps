namespace Users.AppTenants.Constants;

/// <summary>
/// Static class containing error messages specific to AppTenant entity
/// </summary>
public static class AppTenantErrorMessages
{
  /// <summary>
  /// Error messages for AppTenant operations
  /// </summary>
  public static class Operations
  {
    public const string AppTenantNotFound = "AppTenant with ID {0} was not found.";
    public const string AppTenantNotFoundByCode = "AppTenant with code {0} was not found.";
    public const string AppTenantNotFoundByHost = "AppTenant with host {0} was not found.";
    public const string AppTenantAlreadyExists = "An AppTenant with the same {0} already exists.";
    public const string AppTenantCreateFailed = "Failed to create AppTenant.";
    public const string AppTenantUpdateFailed = "Failed to update AppTenant.";
    public const string AppTenantDeleteFailed = "Failed to delete AppTenant.";
    public const string AppTenantInvalidId = "Invalid AppTenant ID provided.";
  }

  /// <summary>
  /// Error messages for AppTenant validation
  /// </summary>
  public static class Validation
  {
    public const string AppTenantNameRequired = "AppTenant name is required.";
    public const string AppTenantNameLength = "AppTenant name must be between {0} and {1} characters.";
    public const string AppTenantHostRequired = "AppTenant host is required.";
    public const string AppTenantHostFormat = "AppTenant host must be a valid domain name.";
    public const string AppTenantHostLength = "AppTenant host must be between {0} and {1} characters.";
    public const string AppTenantTenantCodeRequired = "AppTenant code is required.";
    public const string AppTenantTenantCodeLength = "AppTenant code must be between {0} and {1} characters.";
    public const string AppTenantTenantCodeFormat = "AppTenant code must contain only letters, numbers, and underscores.";
    public const string AppTenantAdminEmailRequired = "Admin email is required.";
    public const string AppTenantAdminEmailFormat = "Admin email must be a valid email address.";
    public const string AppTenantPhoneFormat = "Phone number must be in a valid format.";
    public const string AppTenantDescriptionLength = "Description must not exceed {0} characters.";
    public const string AppTenantAllowedBranchNumberRange = "Allowed branch number must be between {0} and {1}.";
    public const string AppTenantMaxUserCountRange = "Maximum user count must be between {0} and {1}.";
    public const string AppTenantSubscriptionEndDateRequired = "Subscription end date is required.";
    public const string AppTenantSubscriptionEndDateFuture = "Subscription end date must be in the future.";
    public const string AppTenantSubscriptionStartDateBeforeEnd = "Subscription start date must be before end date.";
    public const string AppTenantTenantTypeRequired = "Tenant type is required.";
    public const string AppTenantTenantTypeInvalid = "Invalid tenant type. Valid types are: {0}.";
  }

  /// <summary>
  /// Error messages for AppTenant business rules
  /// </summary>
  public static class Business
  {
    public const string AppTenantCannotDeleteActive = "Cannot delete an active AppTenant.";
    public const string AppTenantCannotDisableLastAdmin = "Cannot disable the last admin AppTenant.";
    public const string AppTenantSubscriptionExpired = "AppTenant subscription has expired.";
    public const string AppTenantMaxUsersReached = "AppTenant has reached its maximum user limit.";
    public const string AppTenantMaxBranchesReached = "AppTenant has reached its maximum branch limit.";
    public const string AppTenantCannotUpdateSubscription = "Cannot update subscription for an inactive AppTenant.";
    public const string AppTenantCannotActivateExpired = "Cannot activate an AppTenant with an expired subscription.";
    public const string AppTenantCannotDeactivateLastAdmin = "Cannot deactivate the last admin AppTenant.";
    public const string AppTenantHostAlreadyInUse = "Host {0} is already in use by another AppTenant.";
    public const string AppTenantTenantCodeAlreadyInUse = "Tenant code {0} is already in use by another AppTenant.";
  }

  /// <summary>
  /// Error messages for AppTenant state transitions
  /// </summary>
  public static class State
  {
    public const string AppTenantAlreadyActive = "AppTenant is already active.";
    public const string AppTenantAlreadyInactive = "AppTenant is already inactive.";
    public const string AppTenantAlreadyEnabledWebUi = "Web UI is already enabled for this AppTenant.";
    public const string AppTenantAlreadyDisabledWebUi = "Web UI is already disabled for this AppTenant.";
    public const string AppTenantCannotActivate = "Cannot activate AppTenant: {0}";
    public const string AppTenantCannotDeactivate = "Cannot deactivate AppTenant: {0}";
    public const string AppTenantCannotEnableWebUi = "Cannot enable Web UI: {0}";
    public const string AppTenantCannotDisableWebUi = "Cannot disable Web UI: {0}";
  }
}