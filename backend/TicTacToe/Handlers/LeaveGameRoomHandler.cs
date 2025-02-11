using MongoDB.Driver.Linq;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Handlers;

public class LeaveGameRoomHandler(
    IRepository<GameRoom> gameRoomRepository,
    IRepository<GameRoomPublic> gameRoomPublicRepository,
    IHasTransactions transactions
    ) : IHandler<LeaveGameRoomRequest, IResult>
{
    public async Task<IResult> Execute(
        LeaveGameRoomRequest request, 
        CancellationToken cancellationToken)
    {
        var gameRoom =
            await gameRoomRepository
                .GetSingleOrDefault(gr => gr.Id.Equals(request.GameRoomId));
        if (gameRoom is null)
            return Results.NotFound("Game room not found");
        
        var alreadyPlayersConnected =
            await gameRoomPublicRepository
                .GetRange()
                .Where(grp => grp.GameRoomId.Equals(gameRoom.Id) 
                              && grp.Status.Equals(PlayerStatus.Player))
                .ToListAsync(cancellationToken: cancellationToken);
        var player = 
            alreadyPlayersConnected.SingleOrDefault(
                p => p.UserName.Equals(
                    request.UserName, StringComparison.OrdinalIgnoreCase));
        if (player is null)
            return Results.NotFound("You're not in the room");

        try
        {
            transactions.BeginTransaction();
            await gameRoomPublicRepository.RemoveAsync(player);
            if (alreadyPlayersConnected.Count - 1 == 0)
            {
                await gameRoomRepository.RemoveAsync(gameRoom);
            }
            else
            {
                gameRoom.BattleState = 0x0;
                gameRoom.State = State.Open;
                gameRoom.LastTurn = string.Empty;
                await gameRoomRepository.UpdateAsync(gameRoom);
            }
            transactions.CommitTransaction();
        }
        catch
        {
            transactions.RollbackTransaction();
            throw;
        }

        return Results.Ok();
    }
}