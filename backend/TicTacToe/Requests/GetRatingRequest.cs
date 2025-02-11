using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record GetRatingRequest(string UserName) : IRequest<IResult>;