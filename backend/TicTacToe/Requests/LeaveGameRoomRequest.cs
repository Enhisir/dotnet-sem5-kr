using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record LeaveGameRoomRequest(Guid GameRoomId, string UserName) : IRequest<IResult>;