using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record SignInRequest : IRequest<IResult>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}