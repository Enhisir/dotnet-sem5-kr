using TicTacToe.Responses;

namespace TicTacToe.Hubs;

public class SignalRMessage
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = null!;
    public GameRoomResponse GameState { get; set; } = null!;
}