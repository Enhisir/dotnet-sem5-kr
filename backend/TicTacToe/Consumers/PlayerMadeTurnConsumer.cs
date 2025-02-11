using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Hubs;
using TicTacToe.Messages;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Consumers;

public class PlayerMadeTurnConsumer(
    IRepository<SystemChatMessage> repository,
    IHubContext<GameRoomHub> hubContext
) : IConsumer<PlayerMadeTurnMessage>
{
    public async Task Consume(ConsumeContext<PlayerMadeTurnMessage> context)
    {
        var msg = context.Message.Value;
        
        var msgId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var text = $"Игрок сходил {msg.PlayerUserName} в клетку {1 + msg.PointY * 3 + msg.PointX }.";
        
        var signalRMessage = new SignalRMessage
        {
            Id = msgId,
            Timestamp = timestamp,
            Message = text,
            GameState = msg.GameRoom
        };

        await repository.AddAsync(new SystemChatMessage
        {
            Id = msgId,
            GameRoomId = msg.GameRoom.Id,
            Timestamp = timestamp,
            Message = text
        });
        await hubContext.Clients
            .Group(msg.GameRoom.Id.ToString())
            .SendAsync("PlayerMadeTurn", signalRMessage);
    }
}