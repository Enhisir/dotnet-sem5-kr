using TicTacToe.Responses;

namespace TicTacToe.Messages;

public class GameEndedMessage
{
    public string Winner { get; set; } = null!;
    public GameRoomResponse Value { get; set; } = null!;
}