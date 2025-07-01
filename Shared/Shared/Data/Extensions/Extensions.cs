using Microsoft.AspNetCore.Identity;
using Shared.Data.Seed;

namespace Shared.Data.Extensions;

public static class Extensions
{
  /// <summary>
  /// Applies any pending migrations for the context to the database.
  /// Will create the database if it does not already exist.
  /// </summary>
  /// <typeparam name="TContext">The type of the db context.</typeparam>
  /// <param name="app">The application builder.</param>
  /// <returns>The application builder.</returns>
  public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
      where TContext : DbContext
  {

    MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
    SeedDataAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
    return app;
  }


  private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
    where TContext : DbContext
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    await context.Database.MigrateAsync();

  }

  private static async Task SeedDataAsync<TContext>(IServiceProvider applicationServices) where TContext : DbContext
  {
    using var scope = applicationServices.CreateScope();
    var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
    foreach (var seeder in seeders)
    {
      await seeder.SeedAllAsync(applicationServices);
    }
  }

}