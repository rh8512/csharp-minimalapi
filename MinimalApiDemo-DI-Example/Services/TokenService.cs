using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MinimalApiDemo;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration) => _configuration = configuration;

    public string CreateToken(string login)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Actor, login)
        };

        var key = _configuration.GetValue<string>("SecurityKey");
        var issuer = _configuration.GetValue<string>("Issuer");
        var audience = _configuration.GetValue<string>("Audience");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
            expires: DateTime.Now.Add(new TimeSpan(0, 5, 0)), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}