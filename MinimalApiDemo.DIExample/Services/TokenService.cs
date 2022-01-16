using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
namespace MinimalApiDemo;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IOptions<TokenSettings> _tokenSettings;

    public TokenService(
        IConfiguration configuration,
        IOptions<TokenSettings> tokenSettings)
    {
        _configuration = configuration;
        _tokenSettings = tokenSettings;
    }

    public string CreateToken(string login)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Actor, login)
        };

        /* Get Token Settings using IConfiguration
        var key = _configuration.GetValue<string>("SecurityKey");
        var issuer = _configuration.GetValue<string>("Issuer");
        var audience = _configuration.GetValue<string>("Audience");
        */

        // Get Token Settings using IOption
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