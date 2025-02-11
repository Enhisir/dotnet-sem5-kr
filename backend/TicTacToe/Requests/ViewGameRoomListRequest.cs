using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record ViewGameRoomListRequest(int Offset, int Limit) : IRequest<IResult>;