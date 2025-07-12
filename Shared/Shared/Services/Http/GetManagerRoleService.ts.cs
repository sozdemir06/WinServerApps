using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;

namespace Shared.Services.Http
{
    public class GetManagerRoleService : IGetManagerRoleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetManagerRoleService> _logger;
        private readonly IDatabase _redis;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetManagerRoleService(
            ILogger<GetManagerRoleService> logger,
            IConfiguration configuration,
            IDatabase redis,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _redis = redis;
            _httpContextAccessor = httpContextAccessor;

            // Configuration'dan base address al
            var baseAddress = configuration.GetSection("ManagerApi:BaseAddress").Value;
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new InvalidOperationException("ManagerApi:BaseAddress configuration is missing");
            }

            // HttpClient oluştur ve ayarla
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            // Default headers ekle
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Timeout ayarla
            var timeoutSeconds = configuration.GetValue<int>("ManagerApi:TimeoutSeconds", 30);
            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        public async Task<IEnumerable<ManagerRoleDto>> GetManagerRolesAsync(Guid managerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"Manager:Roles:{managerId}";
                var cachedData = await _redis.StringGetAsync(cacheKey);
                if (cachedData.HasValue)
                {
                    return JsonSerializer.Deserialize<IEnumerable<ManagerRoleDto>>(cachedData.ToString()) ?? [];
                }

                var accessToken = await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                // 2. HTTP isteği at
                var endpoint = $"/api/user-roles/authorize/{managerId}";
                _logger.LogInformation("Making HTTP request to: {Endpoint}", endpoint);

                var response = await _httpClient.GetFromJsonAsync<GetUserRolesForAuthorizeResult>(endpoint, cancellationToken);
                if (response?.UserRoles == null || !response.UserRoles.Any())
                {
                    _logger.LogWarning("No roles found for manager ID: {ManagerId}", managerId);
                    return [];
                }

                return response.UserRoles ?? [];
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed when getting manager roles for manager ID: {ManagerId}", managerId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred when getting manager roles for manager ID: {ManagerId}", managerId);
                throw;
            }
            finally
            {
                await Task.CompletedTask;
            }
        }

        // Response DTO for the API endpoint
        public record GetUserRolesForAuthorizeResult(IEnumerable<ManagerRoleDto> UserRoles);

    }

}
