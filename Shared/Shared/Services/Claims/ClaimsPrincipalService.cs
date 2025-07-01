
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Services.Claims
{
    public class ClaimsPrincipalService(IHttpContextAccessor httpContextAccessor) : IClaimsPrincipalService
    {
        public Guid GetCurrentTenantId()
        {
            return Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GivenName)?.Value ?? Guid.Empty.ToString());
        }

        public Guid GetCurrentBranchId()
        {
            return Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GroupSid)?.Value ?? Guid.Empty.ToString());
        }

        public Guid GetCurrentUserId()
        {
            return Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        }

        public string GetCurrentUserEmail()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value ?? Guid.Empty.ToString();
        }

        public string GetCurrentUserFullName()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? Guid.Empty.ToString();
        }
    }
}