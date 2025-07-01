namespace Users.UserRoles.Constants;

/// <summary>
/// Static class containing error messages specific to UserRole entity
/// </summary>
public static class UserRoleConstants
{
  /// <summary>
  /// Error messages for UserRole operations
  /// </summary>
  public static class Operations
  {
    public const string UserRoleNotFound = "UserRole with ID {0} was not found.";
    public const string UserRoleAlreadyExists = "User already has this role assigned.";
    public const string UserRoleCreateFailed = "Failed to create UserRole.";
    public const string UserRoleUpdateFailed = "Failed to update UserRole.";
    public const string UserRoleDeleteFailed = "Failed to delete UserRole.";
    public const string UserRoleInvalidId = "Invalid UserRole ID provided.";
  }

  /// <summary>
  /// Error messages for UserRole validation
  /// </summary>
  public static class Validation
  {
    public const string UserIdRequired = "User ID is required.";
    public const string RoleIdRequired = "Role ID is required.";
    public const string UserNotFound = "User with ID {0} was not found.";
    public const string RoleNotFound = "Role with ID {0} was not found.";
    public const string UserInactive = "Cannot assign role to inactive user.";
    public const string RoleInactive = "Cannot assign inactive role to user.";
  }

  /// <summary>
  /// Error messages for UserRole business rules
  /// </summary>
  public static class Business
  {
    public const string CannotAssignSystemRole = "Cannot assign system role to regular user.";
    public const string CannotRemoveLastAdminRole = "Cannot remove the last admin role from user.";
    public const string CannotAssignHigherRole = "Cannot assign a role with higher permission level than your own.";
    public const string CannotModifySystemUserRole = "Cannot modify system user's role.";
  }

  /// <summary>
  /// Error messages for UserRole state transitions
  /// </summary>
  public static class State
  {
    public const string UserRoleAlreadyActive = "UserRole is already active.";
    public const string UserRoleAlreadyInactive = "UserRole is already inactive.";
    public const string CannotActivate = "Cannot activate UserRole: {0}";
    public const string CannotDeactivate = "Cannot deactivate UserRole: {0}";
    public const string CannotUpdate = "Cannot update UserRole: {0}";
    public const string CannotDelete = "Cannot delete UserRole: {0}";
  }
}