using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Extensions
{
  public static class JwtExtensions
  {
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(opt =>
              {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"]!)),
                  ValidateIssuer = true,
                  ValidIssuer = configuration["TokenSettings:Issuer"],
                  ValidateAudience = true,
                  ValidAudience = configuration["TokenSettings:Audience"],
                  ValidateLifetime = true,
                  ClockSkew = TimeSpan.Zero
                };
              });

      return services;
    }

  }
}