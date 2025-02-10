using System.ComponentModel.DataAnnotations;

namespace TicTatToe.Data.Models;

public class GameRoomPublic
{
    public int UserId { get; set; }
    
    public int GameRoomId { get; set; }
}