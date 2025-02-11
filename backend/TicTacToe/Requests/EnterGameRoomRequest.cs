using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public record EnterGameRoomRequest(Guid GameRoomId, string UserName) : IRequest<IResult>;