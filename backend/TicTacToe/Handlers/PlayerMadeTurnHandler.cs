using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Common.CQRS;
using TicTacToe.Helpers;
using TicTacToe.Messages;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;
using TicTatToe.Data.Storage;
using State = TicTatToe.Data.Enum.State;

namespace TicTacToe.Handlers;

public class PlayerMadeTurnHandler(
    IRepository<GameRoom> gameRoomRepository,
    MongoStorage<Rating> ratingStorage,
    IPublishEndpoint publishEndpoint
    // IHubContext<GameRoomHub> hubContext
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

            if (currentPlayerRating.Value > gameRoom.MaxRating)
            {
                await publishEndpoint.Publish(
                    // TODO: костыль, фронтенд узнает своего клиента в сообщении и принудительно дисконнектится.
                    // TODO: Если возможно передлетать, лучше переделать.
                    new PlayerLeftMessage
                    {
                        Value = new GameRoomResponse
                        {
                            Id = gameRoom.Id,
                            CurrentTurn = gameRoom.CurrentTurn,
                            MaxRating = gameRoom.MaxRating,
                            State = gameRoom.State,
                            Users = gameRoom.Players!.Select(p => p.UserName).ToList(),
                        }
                    }, cancellationToken);
            }
            else
                await SetNewGameAsync(gameRoom, cancellationToken);
        }
        else if (TicTacToeHelper.IsGameOver(battleState)) // все поля для победы заполнены
            await SetNewGameAsync(gameRoom, cancellationToken);
        else
            await UpdateGameStateAsync(gameRoom, battleState, request.PointX, request.PointY, cancellationToken);

        return BaseResponse.Success;
    }
    
    private async Task EndGame(
        GameRoom gameRoom, 
        CancellationToken cancellationToken)
    {
        gameRoom.BattleState = 0b00;
        gameRoom.CurrentTurn = gameRoom.Players![Random.Shared.Next(0, 2)].UserName;
        gameRoom.CurrentSign = Sign.X;
        await gameRoomRepository.UpdateAsync(gameRoom);
        await publishEndpoint.Publish(
            new GameStartedMessage
            {
                Value = new GameRoomResponse
                {
                    Id = gameRoom.Id,
                    CurrentTurn = gameRoom.CurrentTurn,
                    MaxRating = gameRoom.MaxRating,
                    State = gameRoom.State,
                    Users = gameRoom.Players.Select(p => p.UserName).ToList(),
                }
            }, cancellationToken);
    }

    private async Task SetNewGameAsync(
        GameRoom gameRoom, 
        CancellationToken cancellationToken)
    {
        gameRoom.BattleState = 0b00;
        gameRoom.CurrentTurn = gameRoom.Players![Random.Shared.Next(0, 2)].UserName;
        gameRoom.CurrentSign = Sign.X;
        await gameRoomRepository.UpdateAsync(gameRoom);
        await publishEndpoint.Publish(
            new GameStartedMessage
            {
                Value = new GameRoomResponse
                {
                    Id = gameRoom.Id,
                    CurrentTurn = gameRoom.CurrentTurn,
                    MaxRating = gameRoom.MaxRating,
                    State = gameRoom.State,
                    Users = gameRoom.Players.Select(p => p.UserName).ToList(),
                }
            }, cancellationToken);
    }
    
    private async Task UpdateGameStateAsync(
        GameRoom gameRoom, 
        int[][] battleState, 
        int x, int y, 
        CancellationToken cancellationToken)
    {
        var prevPlayer = gameRoom.CurrentTurn!;
        gameRoom.BattleState = TicTacToeHelper.Compress(battleState);
        gameRoom.CurrentTurn = 
            gameRoom.Players!
                .SingleOrDefault(p => !p.UserName.Equals(gameRoom.CurrentTurn))!
                .UserName;
        gameRoom.CurrentSign = gameRoom.CurrentSign == Sign.X ? Sign.O : Sign.X;
        await gameRoomRepository.UpdateAsync(gameRoom);
        await publishEndpoint.Publish(
            new PlayerMadeTurnMessage 
            {
                Value =
                {
                    PlayerUserName = prevPlayer,
                    PointX = x,
                    PointY = y,
                    GameRoom = new GameRoomResponse
                    {
                        Id = gameRoom.Id,
                        CurrentTurn = gameRoom.CurrentTurn,
                        MaxRating = gameRoom.MaxRating,
                        State = gameRoom.State,
                        Users = gameRoom.Players!.Select(p => p.UserName).ToList(),
                    }
                }
            }, cancellationToken);
    }
}