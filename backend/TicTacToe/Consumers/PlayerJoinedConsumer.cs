using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Hubs;
using TicTacToe.Messages;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Consumers;

public class PlayerJoinedConsumer(
    IRepository<SystemChatMessage> repository,
    IHubContext<GameRoomHub> hubContext
) : IConsumer<PlayerJoinedMessage>
{
    public Task Consume(ConsumeContext<PlayerJoinedMessage> context)
    {
        throw new NotImplementedException();
    }
    
    public async Task Consume(ConsumeContext<PlayerLeftMessage> context)
    {
        var msg = context.Message;
        
        var msgId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var text = $"{msg.Player} присоединился к игре.";
        
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
            .SendAsync("PlayerJoined", signalRMessage);
    }
}