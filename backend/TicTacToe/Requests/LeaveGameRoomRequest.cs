using TicTacToe.Common.CQRS;
using TicTacToe.Responses;

namespace TicTacToe.Requests;

public record LeaveGameRoomRequest(Guid GameRoomId, string UserName) : IRequest<BaseResponse>;