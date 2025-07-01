namespace Shared.Data.Seed;

/// <summary>
/// Interface for data seeders
/// </summary>
public interface IDataSeeder
{
  /// <summary>
  /// Seeds all data asynchronously
  /// </summary>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>A task that represents the asynchronous operation</returns>
  Task SeedAllAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}