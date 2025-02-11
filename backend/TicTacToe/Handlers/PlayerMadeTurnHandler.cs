using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Handlers;

public class PlayerMadeTurnHandler(
    IRepository<GameRoom> gameRoomRepository,
    IRepository<GameRoomPublic> gameRoomPublicRepository,
    IHasTransactions transactions
) : IHandler<PlayerMadeTurnRequest, BaseResponse>
{
    public Task<BaseResponse> Execute(
        PlayerMadeTurnRequest request, 
        CancellationToken cancellationToken)
    {
        // var gameRoom = request.GameRoom;
        throw new NotImplementedException();
    }
}