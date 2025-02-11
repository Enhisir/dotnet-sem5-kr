using TicTacToe.Responses;

namespace TicTacToe.Messages;

public class GameStartedMessage
{
    public GameRoomResponse Value { get; set; } = null!;
}