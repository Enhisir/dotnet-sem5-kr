using System.Security.Claims;
using TicTacToe.Responses;
using TicTatToe.Data.Models;

namespace TicTacToe.Services.Abstractions;

public interface IAuthService
{
    public ClaimsIdentity GetUserIdentity(User user);
    public SignInCredentials CreateResponse(User user);
    public SignInCredentials CreateResponse(ClaimsIdentity identity);
    public SignInCredentials? Refresh(string refreshToken);
}