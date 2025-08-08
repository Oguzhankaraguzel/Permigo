using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentications;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Conceretes.Authentications;

public class TokenProvider(IConfiguration configuration) 
    : ITokenProvider
{
    public string GenerateToken(AppUser user, IList<string>? roles, DateTime? expiration = null)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
            new Claim(ClaimTypes.GivenName, user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };

        if (roles is not null && roles.Any())
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), 
            Expires = expiration ?? DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
