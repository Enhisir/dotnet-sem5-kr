using Microsoft.EntityFrameworkCore;
using TicTacToe.Common.CQRS;
using TicTacToe.Helpers;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;
using TicTatToe.Data.Storage;

namespace TicTacToe.Handlers;

public class PlayerMadeTurnHandler(
    IRepository<GameRoom> gameRoomRepository,
    MongoStorage<Rating> ratingStorage
    
) : IHandler<PlayerMadeTurnRequest, BaseResponse>
{
    public async Task<BaseResponse> Execute(
        PlayerMadeTurnRequest request, 
        CancellationToken cancellationToken)
    {
        var gameRoom =
            await gameRoomRepository
                .GetRange()
                .Where(gr => gr.Id.Equals(request.GameRoomId))
                .Include(gr => gr.Players)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
        if (gameRoom is null) return BaseResponse.Failure("Game room not found");
        if (!gameRoom.State.Equals(State.Running) 
            || !gameRoom.CurrentTurn!.Equals(request.UserName))
            return BaseResponse.Failure("You cannot make your turn for now");
        
        var currentPlayer = 
            gameRoom.Players!
                .SingleOrDefault(p => p.UserName.Equals(gameRoom.CurrentTurn))!;
        var nextPlayer = 
            gameRoom.Players!
                .SingleOrDefault(p => !p.UserName.Equals(gameRoom.CurrentTurn))!;
        
        var battleState = TicTacToeHelper.Decompress(gameRoom.BattleState);
        battleState[request.PointY][request.PointX] = (int)gameRoom.CurrentSign!;
        
        if (TicTacToeHelper.CheckWin(battleState).Equals(gameRoom.CurrentSign))
        {
            var currentPlayerRating = 
                await ratingStorage
                    .GetOneAsync(r => r.UserName.Equals(currentPlayer.UserName));
            var nextPlayerRating = 
                await ratingStorage
                    .GetOneAsync(r => r.UserName.Equals(nextPlayer.UserName));
            currentPlayerRating!.Value += 3;
            nextPlayerRating!.Value -= 1;
            await ratingStorage.UpdateAsync(
                p => p.UserName.Equals(gameRoom.CurrentTurn), 
                currentPlayerRating);
            await ratingStorage.UpdateAsync(
                p => !p.UserName.Equals(gameRoom.CurrentTurn), 
                nextPlayerRating);
            
            // выкинуть чела при достижении лимита по рейтингу
        }
        else if (TicTacToeHelper.IsGameOver(battleState)) // все поля для победы заполнены
        {
            await SetNewGameAsync(gameRoom);
        }
        else // продолжаем; выиграть может только тот, кто ходит
        {
            await UpdateGameStateAsync(gameRoom, battleState);
        }

        throw new NotImplementedException();
    }

    private async Task SetNewGameAsync(GameRoom gameRoom)
    {
        gameRoom.BattleState = 0b00;
        gameRoom.CurrentTurn = gameRoom.Players![Random.Shared.Next(0, 2)].UserName;
        gameRoom.CurrentSign = Sign.X;
        await gameRoomRepository.UpdateAsync(gameRoom);
        // разослать сообщение
    }
    
    private async Task UpdateGameStateAsync(GameRoom gameRoom, int[][] battleState)
    {
        gameRoom.BattleState = TicTacToeHelper.Compress(battleState);
        gameRoom.CurrentTurn = 
            gameRoom.Players!
                .SingleOrDefault(p => !p.UserName.Equals(gameRoom.CurrentTurn))!
                .UserName;
        gameRoom.CurrentSign = gameRoom.CurrentSign == Sign.X ? Sign.O : Sign.X;
        await gameRoomRepository.UpdateAsync(gameRoom);
        // разослать сообщение
    }
}