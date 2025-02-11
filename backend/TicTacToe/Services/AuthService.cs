using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.Common.Helpers.Abstractions;
using TicTatToe.Data.Models;
using TicTacToe.Responses;
using TicTacToe.Services.Abstractions;

namespace TicTacToe.Services;

public class AuthService(
    IJwtService jwtService
    ) : IAuthService
{
    public SignInCredentials? Refresh(string refreshToken)
    {
        try
        {
            var identity = jwtService.ValidateToken(refreshToken);
            return CreateResponse(identity);
        }
        catch (SecurityTokenException)
        {
            return null;
        }
    }

    public SignInCredentials CreateResponse(User user)
        => CreateResponse(GetUserIdentity(user));

    public SignInCredentials CreateResponse(ClaimsIdentity identity)
    {
        var accessTokenExpires = TimeSpan.FromDays(1);
        var refreshTokenExpires = TimeSpan.FromDays(30);
        var accessToken = jwtService.GenerateToken(identity, accessTokenExpires);
        var refreshToken = jwtService.GenerateToken(identity, refreshTokenExpires);

        return new SignInCredentials(
            identity.Name!, 
            accessToken, 
            accessTokenExpires.TotalMilliseconds, 
            refreshToken);
    }

    public ClaimsIdentity GetUserIdentity(User user)
        => new([new Claim(ClaimTypes.Name, user.UserName)]);
}