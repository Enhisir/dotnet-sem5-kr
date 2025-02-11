using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToe.Common.Helpers;

public class JwtOptions(IConfiguration configuration)
{
    public readonly string ValidIssuer = configuration["Jwt:Issuer"]!;
    public readonly string ValidAudience = configuration["Jwt:Issuer"]!;
    public readonly SymmetricSecurityKey IssuerSigningKey =
        new(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
    public readonly bool ValidateIssuer = true;
    public readonly bool ValidateAudience = true;
    public readonly bool ValidateLifetime = false;
    public readonly bool ValidateIssuerSigningKey = true;
}