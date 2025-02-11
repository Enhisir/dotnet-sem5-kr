using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Hubs;
using TicTacToe.Messages;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Consumers;

public class GameStartedConsumer(
    IRepository<SystemChatMessage> repository,
    IHubContext<GameRoomHub> hubContext
    ) : IConsumer<GameStartedMessage>
{
    public async Task Consume(ConsumeContext<GameStartedMessage> context)
    {
        var msg = context.Message;
        
        var msgId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var text = "Игра началась!";
        
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
            .SendAsync("GameStarted", signalRMessage);
    }
}