using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.Common.Helpers.Abstractions;

namespace TicTacToe.Common.Helpers;

public class JwtService(JwtOptions jwtOptions) : IJwtService
{
    private readonly TokenValidationParameters _validationParameters = new()
    {
        ValidAudience = jwtOptions.ValidAudience,
        ValidIssuer = jwtOptions.ValidIssuer,
        IssuerSigningKey = jwtOptions.IssuerSigningKey,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
    };

    public string GenerateToken(ClaimsIdentity userIdentity, TimeSpan expires)
    {
        var token = new JwtSecurityToken(
            issuer: jwtOptions.ValidIssuer,
            audience: jwtOptions.ValidAudience,
            userIdentity.Claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.Add(expires),
            signingCredentials: new SigningCredentials(
                jwtOptions.IssuerSigningKey,
                SecurityAlgorithms.HmacSha256)
        );
        var jwtHandler = new JwtSecurityTokenHandler();
        return jwtHandler.WriteToken(token);
    }

    public ClaimsIdentity ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, _validationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return new ClaimsIdentity(principal.Claims);
    }
}