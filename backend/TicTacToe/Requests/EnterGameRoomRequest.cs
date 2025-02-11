using TicTacToe.Common.CQRS;
using TicTacToe.Responses;

namespace TicTacToe.Requests;

public record EnterGameRoomRequest(Guid GameRoomId, string UserName) : IRequest<BaseResponse>;