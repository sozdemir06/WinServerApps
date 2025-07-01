namespace Shared.Services.Claims
{
    public interface IClaimsPrincipalService
    {
        Guid GetCurrentTenantId();
        Guid GetCurrentBranchId();
        Guid GetCurrentUserId();
        string GetCurrentUserEmail();
        string GetCurrentUserFullName();
    }
}