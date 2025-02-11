using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Common.CQRS;
using TicTacToe.Messages;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTacToe.Services.Abstractions;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Hubs;

[Authorize]
public class GameRoomHub(
    ILogger<GameRoomHub> logger,
    IMediator mediator,
    IRepository<GameRoom> gameRoomRepository,
    IRepository<GameRoomPublic> gameRoomPublicRepository,
    IPublishEndpoint publishEndpoint)
    : Hub<IGameRoomClient>
{
    public override async Task OnConnectedAsync()
    {
        var username = Context.GetHttpContext()!.User.Identity!.Name!;
        var gameRoomIdUnparsed = Context
            .GetHttpContext()!.Request.Query["gameRoomId"]
            .FirstOrDefault()
            ?.Trim();
        if (!Guid.TryParse(gameRoomIdUnparsed, out var gameRoomId)
            || gameRoomId == Guid.Empty)
        {
            logger.LogError("Could not get game room id for user: {username}", username);
            Context.Abort();
            return;
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, gameRoomIdUnparsed);
        
        var player = 
            await gameRoomPublicRepository
                .GetSingleOrDefault(g => 
                        g.UserName.Equals(username) 
                        && g.GameRoomId.Equals(gameRoomId));
        var playerStatus = player is null // игрок создал комнату?
            ? PlayerStatus.Observer 
            : PlayerStatus.Player;
        _ = new GameRoomHubContextItemsHelper(Context.Items) // comfortable dictionary filling
        {
            GameRoomId = gameRoomId,
            UserName = username,
            Status = playerStatus
        };
        
        var gameRoom =
            await gameRoomRepository
                .GetRange()
                .Where(gr => gr.Id.Equals(gameRoomId))
                .Include(gr => gr.Players)
                .SingleOrDefaultAsync();
        await publishEndpoint.Publish(
            new PlayerJoinedMessage
            {
                Player = username,
                Value = new GameRoomResponse
                {
                    Id = gameRoom!.Id,
                    CurrentTurn = gameRoom.CurrentTurn,
                    MaxRating = gameRoom.MaxRating,
                    State = gameRoom.State,
                    Users = gameRoom.Players!.Select(p => p.UserName).ToList(),
                }
            });
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception) // Leave
    {
        var items = new GameRoomHubContextItemsHelper(Context.Items);
        var response = await mediator.Send(
            new LeaveGameRoomRequest(items.GameRoomId, items.UserName));
        
        if (!response.IsSuccess)
        {
            Context.Abort();
            logger.LogWarning(
                "OnDisconnectedAsync ended up with failure: {responseMessage}", 
                response.Message);
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task EnterGameRoomAsync()
    {
        var items = new GameRoomHubContextItemsHelper(Context.Items);
        
        var response = await mediator.Send(new EnterGameRoomRequest(items.GameRoomId, items.UserName));
        if (!response.IsSuccess)
        {
            Context.Abort();
            logger.LogWarning(
                "EnterGameRoomAsync ended up with failure: {responseMessage}", 
                response.Message);
        }
    }
    
    public async Task PlayerMadeTurn(int x, int y)
    {
        var items = new GameRoomHubContextItemsHelper(Context.Items);
        
        var response = await mediator.Send(
            new PlayerMadeTurnRequest(items.GameRoomId, items.UserName, x, y));
        if (!response.IsSuccess)
        {
            Context.Abort();
            logger.LogWarning(
                "EnterGameRoomAsync ended up with failure: {responseMessage}", 
                response.Message);
        }
    }
}