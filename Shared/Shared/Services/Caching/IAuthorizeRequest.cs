

namespace Shared.Services.Caching
{
    public interface IAuthorizeRequest
    {
        List<string> PermissionRoles { get; }
    }
}