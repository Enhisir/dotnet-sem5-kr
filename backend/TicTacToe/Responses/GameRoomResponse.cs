using TicTatToe.Data.Enum;

namespace TicTacToe.Responses;

public class GameRoomResponse
{
    public Guid Id { get; set; }
    
    public State State { get; set; }

    public int[][] BattleState { get; } = [
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0]
    ];
    
    public int MaxRating { get; set; }
    
    public string? CurrentTurn { get; set; }

    public List<string> Users { get; set; } = null!;
}