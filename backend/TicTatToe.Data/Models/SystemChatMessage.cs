using System.ComponentModel.DataAnnotations;

namespace TicTatToe.Data.Models;

public class SystemChatMessage
{
    public Guid Id { get; set; }
    
    public Guid GameRoomId { get; set; }
    
    public DateTime Timestamp { get; set; }

    [MaxLength(256)]
    public string Message { get; set; } = null!;
}