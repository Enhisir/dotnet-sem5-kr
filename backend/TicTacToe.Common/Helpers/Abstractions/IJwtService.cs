using System.Security.Claims;

namespace TicTacToe.Common.Helpers.Abstractions;

public interface IJwtService
{
    public string GenerateToken(ClaimsIdentity userIdentity, TimeSpan expires);
    public ClaimsIdentity ValidateToken(string token);
}