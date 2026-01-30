using BankRUs.Application;
using BankRUs.Application.Authentication;
using BankRUs.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace BankRUs.Intrastructure.Autentication;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwt;

    public TokenService(IOptions<JwtOptions> options)
    {
        _jwt = options.Value;
    }


    public Token CreateToken(string UserId, string Email)
    {
        // 1 - Lägg till claims
        var claims = new List<Claim>
        {
            // Claims som har med användaren att göra
            new(JwtRegisteredClaimNames.Sub, UserId),
            new(JwtRegisteredClaimNames.Email, Email),
            new(JwtRegisteredClaimNames.PreferredUsername, Email),
        };

        var issuer = _jwt.Issuer;
        var audience = _jwt.Audience;
        var secret = _jwt.Secret;
        var expiresMinutes = _jwt.ExpiresMinutes;

        // Hämta från appsettings.json
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Hämta från appsettings.json
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);
        // new(JwtRegisteredClaimNames.Exp, "1769686438"),

        var jwt = new JwtSecurityToken(
          issuer: issuer,
          audience: audience,
          claims: claims,
          expires: expiresAtUtc,
          signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new Token(
            AccessToken: tokenString,
            ExpiresAtUtc: DateTime.UtcNow.AddHours(1));
    }
}
