using Shared.Languages;
using Shared.Services.Claims;
using Shared.Services.Http;

namespace Shared.Behaviors
{
    public class AuthoırizeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizeRequest
    {
        private readonly IGetManagerRoleService _getManagerRoleService;
        private readonly IClaimsPrincipalService _claimsPrincipalService;
        private readonly ILocalizationService _localizationService;

        public AuthoırizeBehavior(
                IGetManagerRoleService getManagerRoleService,
                IClaimsPrincipalService claimsPrincipalService,
                ILocalizationService localizationService
                )
        {
            _getManagerRoleService = getManagerRoleService;
            _claimsPrincipalService = claimsPrincipalService;
            _localizationService = localizationService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var permissionRoles = request.PermissionRoles.Select(x => x.ToUpperInvariant()).ToList();

            var userRoles = await _getManagerRoleService.GetManagerRolesAsync(_claimsPrincipalService.GetCurrentUserId(), cancellationToken) ?? [];

            var userRolesNames = userRoles.Select(x => x.Name.ToUpperInvariant()).ToList() ?? [];
            var isAuthorized = userRolesNames.Any(x => permissionRoles.Contains(x));

            if (isAuthorized)
            {
                return await next(cancellationToken);
            }
            throw new UnauthorizedAccessException(await _localizationService.Translate("Unauthorized"));

        }
    }
}