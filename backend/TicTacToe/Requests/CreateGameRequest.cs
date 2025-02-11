using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record CreateGameRequest(string UserName, uint MaxRating) : IRequest<IResult>;