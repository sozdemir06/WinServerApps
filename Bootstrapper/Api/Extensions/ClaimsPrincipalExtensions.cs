using System.Security.Claims;

namespace Bootstrapper.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
  public static Guid GetId(this ClaimsPrincipal principal)
  {
    var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
    return claim != null ? Guid.Parse(claim.Value) : throw new InvalidOperationException("ID claim not found");
  }

  public static string GetEmail(this ClaimsPrincipal principal)
  {
    return principal.FindFirst(ClaimTypes.Email)?.Value ?? throw new InvalidOperationException("Email claim not found");
  }

  public static string GetFullName(this ClaimsPrincipal principal)
  {
    return principal.FindFirst(ClaimTypes.Name)?.Value ?? throw new InvalidOperationException("Name claim not found");
  }

  public static Guid GetTenantId(this ClaimsPrincipal principal)
  {
    var claim = principal.FindFirst(ClaimTypes.GivenName);
    return claim != null ? Guid.Parse(claim.Value) : throw new InvalidOperationException("Tenant ID claim not found");
  }

  public static Guid GetBranchId(this ClaimsPrincipal principal)
  {
    var claim = principal.FindFirst(ClaimTypes.GroupSid);
    return claim != null ? Guid.Parse(claim.Value) : throw new InvalidOperationException("Branch ID claim not found");
  }

  public static bool IsAuthenticated(this ClaimsPrincipal principal)
  {
    return principal.Identity?.IsAuthenticated == true &&
           principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier);
  }
}