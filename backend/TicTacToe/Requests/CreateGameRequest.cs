using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record CreateGameRequest(string UserName, int MaxRating) : IRequest<IResult>;