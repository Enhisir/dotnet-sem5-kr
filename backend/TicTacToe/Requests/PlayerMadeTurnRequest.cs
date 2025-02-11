using TicTacToe.Common.CQRS;
using TicTacToe.Responses;

namespace TicTacToe.Requests;

public record PlayerMadeTurnRequest(int PointX, int PointY) : IRequest<BaseResponse>; // а потому что настоящий респонс пойдет через рэббит