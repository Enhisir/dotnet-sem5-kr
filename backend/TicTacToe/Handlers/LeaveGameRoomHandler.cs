using MongoDB.Driver.Linq;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Handlers;

public class LeaveGameRoomHandler(
    IRepository<GameRoom> gameRoomRepository,
    IRepository<GameRoomPublic> gameRoomPublicRepository,
    IHasTransactions transactions
    ) : IHandler<LeaveGameRoomRequest, BaseResponse>
{
    public async Task<BaseResponse> Execute(
        LeaveGameRoomRequest request, 
        CancellationToken cancellationToken)
    {
        var gameRoom =
            await gameRoomRepository
                .GetSingleOrDefault(gr => gr.Id.Equals(request.GameRoomId));
        if (gameRoom is null)
            return BaseResponse.Failure("Game room not found");
        
        var alreadyPlayersConnected =
            await gameRoomPublicRepository
                .GetRange()
                .Where(grp => grp.GameRoomId.Equals(gameRoom.Id))
                .ToListAsync(cancellationToken: cancellationToken);
        var player = 
            alreadyPlayersConnected.SingleOrDefault(
                p => p.UserName.Equals(
                    request.UserName, StringComparison.OrdinalIgnoreCase));
        if (player is null)
            return BaseResponse.Failure("You're not in the room");

        try
        {
            transactions.BeginTransaction();
            await gameRoomPublicRepository.RemoveAsync(player);
            gameRoom.BattleState = 0b00;
            gameRoom.CurrentTurn = string.Empty;
            gameRoom.CurrentSign = Sign.Empty;
            gameRoom.State = alreadyPlayersConnected.Count - 1 == 0 
                ? State.Closed 
                : State.Open;
            await gameRoomRepository.UpdateAsync(gameRoom);
            transactions.CommitTransaction();
        }
        catch
        {
            transactions.RollbackTransaction();
            throw;
        }
        
        // Сообщить всем что вышел игрок такой-то
        // Через рэббит

        return BaseResponse.Success;
    }
}