namespace Users.AppRoles.Constants;

/// <summary>
/// Static class containing error messages specific to AppRole entity
/// </summary>
public static class AppRoleConstants
{
  /// <summary>
  /// Error messages for AppRole operations
  /// </summary>
  public static class Operations
  {
    public const string AppRoleNotFound = "AppRole with ID {0} was not found.";
    public const string AppRoleNotFoundByName = "AppRole with name {0} was not found.";
    public const string AppRoleAlreadyExists = "An AppRole with the same name {0} already exists.";
    public const string AppRoleCreateFailed = "Failed to create AppRole.";
    public const string AppRoleUpdateFailed = "Failed to update AppRole.";
    public const string AppRoleDeleteFailed = "Failed to delete AppRole.";
    public const string AppRoleInvalidId = "Invalid AppRole ID provided.";
  }

  /// <summary>
  /// Error messages for AppRole validation
  /// </summary>
  public static class Validation
  {
    public const string AppRoleNameRequired = "AppRole name is required.";
    public const string AppRoleNameLength = "AppRole name must be between {0} and {1} characters.";
    public const string AppRoleDescriptionLength = "Description must not exceed {0} characters.";
    public const string AppRoleTypeRequired = "Role type is required.";
    public const string AppRoleTypeInvalid = "Invalid role type. Valid types are: {0}.";
    public const string AppRolePermissionLevelRange = "Permission level must be between {0} and {1}.";
    public const string AppRolePermissionLevelRequired = "Permission level is required.";
    public const string AppRoleNameFormat = "Role name must contain only letters, numbers, spaces, and underscores.";
    public const string AppRoleDescriptionFormat = "Description can only contain letters, numbers, spaces, and basic punctuation.";
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 500;
    public const int MinPermissionLevel = 1;
    public const int MaxPermissionLevel = 100;
  }

  /// <summary>
  /// Error messages for AppRole business rules
  /// </summary>
  public static class Business
  {
    public const string AppRoleCannotDeleteSystem = "Cannot delete a system role.";
    public const string AppRoleCannotDeleteAssigned = "Cannot delete a role that is assigned to users.";
    public const string AppRoleCannotModifySystem = "Cannot modify a system role.";
    public const string AppRoleCannotDeactivateSystem = "Cannot deactivate a system role.";
    public const string AppRoleCannotUpdatePermissionLevel = "Cannot update permission level of a system role.";
    public const string AppRoleCannotAssignHigherLevel = "Cannot assign a role with higher permission level than your own.";
    public const string AppRoleCannotRemoveLastAdmin = "Cannot remove the last admin role.";
    public const string AppRoleNameAlreadyInUse = "Role name {0} is already in use by another role.";
  }

  /// <summary>
  /// Error messages for AppRole state transitions
  /// </summary>
  public static class State
  {
    public const string AppRoleAlreadyActive = "AppRole is already active.";
    public const string AppRoleAlreadyInactive = "AppRole is already inactive.";
    public const string AppRoleCannotActivate = "Cannot activate AppRole: {0}";
    public const string AppRoleCannotDeactivate = "Cannot deactivate AppRole: {0}";
    public const string AppRoleCannotUpdate = "Cannot update AppRole: {0}";
    public const string AppRoleCannotDelete = "Cannot delete AppRole: {0}";
  }

  public static class RoleTypes
  {
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string User = "User";
    public const string Guest = "Guest";
  }

  public static class PermissionLevels
  {
    public const int SuperAdmin = 100;
    public const int Admin = 80;
    public const int Manager = 60;
    public const int User = 40;
    public const int Guest = 20;
  }
}