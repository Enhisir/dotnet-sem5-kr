using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public class SignUpRequest : IRequest<IResult>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}