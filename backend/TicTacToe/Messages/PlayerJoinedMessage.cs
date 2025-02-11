using TicTacToe.Responses;

namespace TicTacToe.Messages;

public class PlayerJoinedMessage
{
    public string Player { get; set; } = null!;
    public GameRoomResponse Value { get; set; } = null!;
}