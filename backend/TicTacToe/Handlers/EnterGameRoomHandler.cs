using Microsoft.EntityFrameworkCore;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories;
using TicTatToe.Data.Storage;

namespace TicTacToe.Handlers;

public class EnterGameRoomHandler(
    IRepository<GameRoom> gameRoomRepository,
    IRepository<GameRoomPublic> gameRoomPublicRepository,
    MongoStorage<Rating> ratingStorage
    ) : IHandler<EnterGameRoomRequest, IResult>
{
    public async Task<IResult> Execute(
        EnterGameRoomRequest request, 
        CancellationToken cancellationToken)
    {
        var maybePlaying =
            await gameRoomPublicRepository
                .GetSingleOrDefault(grp => grp.UserName.Equals(request.UserName));
        if (maybePlaying is not null)
            return Results.Conflict("You're already in game room");
        
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
                .CountAsync(cancellationToken: cancellationToken);
        if (alreadyPlayersConnected == 2)
            return Results.Conflict("Room is already full");
        
        var rating = 
            await ratingStorage
                .GetOneAsync(r => r.UserName.Equals(request.UserName));
        if (rating is null || rating.Value > gameRoom.MaxRating)
            return Results.Forbid(); // "your rating is more than maximum acceptable in this room"
        
        var gameRoomPublic = new GameRoomPublic()
        {
            GameRoomId = gameRoom.Id,
            UserName = request.UserName
        };
        await gameRoomPublicRepository.AddAsync(gameRoomPublic);
        
        // start game logic
        
        return Results.Ok();
    }
}