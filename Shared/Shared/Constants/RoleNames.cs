namespace Shared.Constants;

/// <summary>
/// Static class containing role name constants used across the application
/// </summary>
public static class RoleNames
{
  /// <summary>
  /// System administrator role with full access
  /// </summary>
  public const string SystemAdmin = "SystemAdmin";

  /// <summary>
  /// Tenant administrator role with access to tenant management
  /// </summary>
  public const string TenantAdmin = "TenantAdmin";


  /// <summary>
  /// Regular user role with basic access
  /// </summary>
  public const string Customer = "Customer";

  /// <summary>
  /// Read-only role with limited access
  /// </summary>
  public const string Manager = "Manager";

  /// <summary>
  /// Gets all predefined role names dynamically using reflection
  /// </summary>
  public static IEnumerable<string> GetAllRoles()
  {
    return typeof(RoleNames)
      .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
      .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
      .Select(f => (string)f.GetValue(null)!)
      .OrderBy(r => r);
  }
}