using TicTacToe.Common.CQRS;
using TicTacToe.Responses;

namespace TicTacToe.Requests;

public record PlayerMadeTurnRequest(
    Guid GameRoomId, 
    string UserName, 
    int PointX, 
    int PointY) : IRequest<BaseResponse>; 
// а потому что настоящий респонс пойдет через рэббит