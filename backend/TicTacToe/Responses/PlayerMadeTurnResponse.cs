namespace TicTacToe.Responses;

public class PlayerMadeTurnResponse
{
    public string PlayerUserName { get; set; } = null!;
    public int PointX { get; set; }
    public int PointY { get; set; }
    public GameRoomResponse GameRoom { get; set; } = null!;
}