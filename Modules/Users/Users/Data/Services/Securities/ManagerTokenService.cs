using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Users.Managers.Models;
using Users.UserRoles.Features.GetUserRolesByUserId;
using Users.UserRoles.Features.GetUserRolesForAuthorize;

namespace Users.Data.Services.Securities
{
  public class ManagerTokenService : IManagerTokenService
  {
    private readonly IConfiguration _configuration;
    private readonly ISender _sender;
    private readonly string _jwtSecret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _tokenExpirationMinutes;

    public ManagerTokenService(IConfiguration configuration, ISender sender)
    {
      _configuration = configuration;
      _sender = sender;
      _jwtSecret = _configuration["TokenSettings:Key"] ?? throw new ArgumentNullException("Jwt:Secret configuration is missing");
      _issuer = _configuration["TokenSettings:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer configuration is missing");
      _audience = _configuration["TokenSettings:Audience"] ?? throw new ArgumentNullException("Jwt:Audience configuration is missing");
      _tokenExpirationMinutes = int.Parse(_configuration["TokenSettings:TokenExpiration"] ?? "1");
    }

    public async Task<string> CreateTokenAsync(Manager manager)
    {

      await _sender.Send(new GetUserRolesForAuthorizeQuery(manager.Id, _tokenExpirationMinutes));

      var tokenHandler = new JwtSecurityTokenHandler();

      var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, manager.Id.ToString()),
                new(ClaimTypes.Email, manager.Email),
                new(ClaimTypes.Name, $"{manager.FirstName} {manager.LastName}"),
                new(ClaimTypes.GivenName,manager.TenantId.ToString()!),
                new(ClaimTypes.GroupSid,manager.BranchId.ToString()!),
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(_tokenExpirationMinutes),
        SigningCredentials = creds,
        Issuer = _issuer,
        Audience = _audience,
        

      };


      var token = tokenHandler.CreateToken(tokenDescriptor);

      return await Task.FromResult(tokenHandler.WriteToken(token));
    }
  }
}