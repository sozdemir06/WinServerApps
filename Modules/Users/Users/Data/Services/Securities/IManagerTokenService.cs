using System.Threading.Tasks;
using Users.Managers.Models;

namespace Users.Data.Services.Securities
{
  /// <summary>
  /// Interface for token generation and management services
  /// </summary>
  public interface IManagerTokenService
  {
    /// <summary>
    /// Creates a new token asynchronously
    /// </summary>
    /// <param name="userId">The user identifier for whom the token is being created</param>
    /// <param name="additionalClaims">Optional additional claims to include in the token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated token string.</returns>
    Task<string> CreateTokenAsync(Manager manager);
  }
}