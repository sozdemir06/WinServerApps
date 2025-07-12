namespace Shared.Extensions;

public static class CorsExtensions
{

  public static IServiceCollection AddCustomCors(this IServiceCollection services)
  {
    services.AddCors();

    return services;
  }

  public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
  {
    app.UseCors(opt =>
    {
      opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000","http://localhost:3001");
    });
    return app;
  }
}