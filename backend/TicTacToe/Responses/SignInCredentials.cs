namespace TicTacToe.Responses;

public record SignInCredentials(
    string UserName,
    string AccessToken,
    double AccessTokenExpires,
    string RefreshToken);