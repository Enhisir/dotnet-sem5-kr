using TicTacToe.Responses;

namespace TicTacToe.Messages;

public class PlayerMadeTurnMessage
{
    public PlayerMadeTurnResponse Value { get; set; } = null!;
}