namespace Shared.Constants;

/// <summary>
/// Static class containing role name constants used across the application
/// </summary>
public static class RoleNames
{
  public const string SystemAdmin = "SystemAdmin";
  public const string TenantAdmin = "TenantAdmin";
  public const string Customer = "Customer";
  public const string Manager = "Manager";
  public const string BranchRead = "BranchRead";
  public const string BranchEdit = "BranchEdit";
  public const string BranchDelete = "BranchDelete";
  public const string TenantManagerRead = "TenantManagerRead";
  public const string TenantManagerEdit = "TenantManagerEdit";
  public const string TenantManagerDelete = "TenantManagerDelete";

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