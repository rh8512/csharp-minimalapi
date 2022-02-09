using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
namespace MinimalApiDemo;

public class TokenService : ITokenService
{
    private readonly IOptions<TokenSettings> _tokenSettings;

    public TokenService(
        IOptions<TokenSettings> tokenSettings)
    {
        _tokenSettings = tokenSettings;
    }

    public string CreateToken(string login)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Actor, login)
        };

        var key = _tokenSettings.Value.SecurityKey;
        var issuer = _tokenSettings.Value.Issuer;
        var audience = _tokenSettings.Value.Audience;
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
            expires: DateTime.Now.Add(new TimeSpan(0, 5, 0)), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}