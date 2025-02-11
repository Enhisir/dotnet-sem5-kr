using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Hubs;
using TicTacToe.Messages;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Consumers;

public class GameEndedConsumer(
    IRepository<SystemChatMessage> repository,
    IHubContext<GameRoomHub> hubContext
    ) : IConsumer<GameEndedMessage>
{
    public async Task Consume(ConsumeContext<GameEndedMessage> context)
    {
        var msg = context.Message;
        
        var msgId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var text = $"Игра окончена. Победил {msg.Winner}!";
        
        var signalRMessage = new SignalRMessage
        {
            Id = msgId,
            Timestamp = timestamp,
            Message = text,
            GameState = msg.Value
        };

        await repository.AddAsync(new SystemChatMessage
        {
            Id = msgId,
            GameRoomId = msg.Value.Id,
            Timestamp = timestamp,
            Message = text
        });
        await hubContext.Clients
            .Group(msg.Value.Id.ToString())
            .SendAsync("GameEnded", signalRMessage);
    }
}