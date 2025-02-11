using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record RefreshRequest(string RefreshToken) : IRequest<IResult>;