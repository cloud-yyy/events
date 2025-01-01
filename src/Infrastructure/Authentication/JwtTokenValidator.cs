using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtTokenValidator(
    IOptions<JwtOptions> options
) : IJwtTokenValidator
{
    private readonly JwtOptions _options = options.Value;

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,

            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
        };

        SecurityToken securityToken;
        ClaimsPrincipal? principal;

        try
        {
            principal = tokenHandler
                .ValidateToken(token, parameters, out securityToken);
        }
        catch
        {
            return null;
        }

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is not null && jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase))
        {
            return principal;
        }
        
        return null;
    }
}
