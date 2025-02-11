namespace TicTacToe.Responses;

public class PlayerMadeTurnResponse
{
    public string PlayerUserName { get; set; } = null!;
    public string PointX { get; set; } = null!;
    public string PointY { get; set; } = null!;
    public GameRoomResponse GameRoom { get; set; } = null!;
}