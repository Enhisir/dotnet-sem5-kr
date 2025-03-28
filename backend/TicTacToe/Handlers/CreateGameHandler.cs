using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Handlers;

public class CreateGameHandler(
    IRepository<GameRoom> gameRoomRepository,
    IRepository<GameRoomPublic> gameRoomPublicRepository
) : IHandler<CreateGameRequest, IResult>
{
    public async Task<IResult> Execute(
        CreateGameRequest request, 
        CancellationToken cancellationToken)
    {
        var maybePlaying =
            await gameRoomPublicRepository
                .GetSingleOrDefault(grp => grp.UserName.Equals(request.UserName));
        
        if (maybePlaying is not null)
            return Results.Conflict("You're already playing in another room!");

        var newGame = new GameRoom
        {
            Id = Guid.NewGuid(),
            State = State.Open,
            BattleState = 0b00,
            CreatedAt = DateTime.UtcNow.Date,
            CurrentTurn = string.Empty,
            CurrentSign = Sign.Empty,
            MaxRating = request.MaxRating
        };
        var firstPlayer = new GameRoomPublic
        {
            GameRoomId = newGame.Id, 
            UserName = request.UserName
        };
        await gameRoomRepository.AddAsync(newGame);
        await gameRoomPublicRepository.AddAsync(firstPlayer);
        
        return Results.Ok(new CreateGameResponse(newGame.Id));
    }
}