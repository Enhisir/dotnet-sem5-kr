using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record CreateGameRequest(int maxRating) : IRequest;