using TicTatToe.Data.Enum;

namespace TicTatToe.Data.Models;

public class GameRoom
{
    public Guid Id { get; set; }
    
    public State State { get; set; }
    
    public uint BattleState { get; set; }
    
    public uint MaxRating { get; set; }
    
    public string? LastTurn { get; set; }
}