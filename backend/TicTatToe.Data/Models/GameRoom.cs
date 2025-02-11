using TicTatToe.Data.Enum;

namespace TicTatToe.Data.Models;

public class GameRoom
{
    public Guid Id { get; set; }
    
    public State State { get; set; }
    
    public int BattleState { get; set; }
    
    public int MaxRating { get; set; }

    public string? CurrentTurn { get; set; }
    
    public Sign? CurrentSign { get; set; }
    
    public List<GameRoomPublic>? Players { get; set; }
}