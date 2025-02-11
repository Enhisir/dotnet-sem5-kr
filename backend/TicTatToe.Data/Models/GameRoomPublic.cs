using System.ComponentModel.DataAnnotations;
using TicTatToe.Data.Enum;

namespace TicTatToe.Data.Models;

public class GameRoomPublic
{
    [Length(6, 200)]
    public string UserName { get; set; } = null!;
    
    public Guid GameRoomId { get; set; }
    
    public PlayerStatus Status { get; set; }
}