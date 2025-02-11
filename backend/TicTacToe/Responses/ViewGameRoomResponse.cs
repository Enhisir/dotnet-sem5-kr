using TicTatToe.Data.Models;

namespace TicTacToe.Responses;

public class ViewGameRoomResponse
{
    public Guid Id { get; init; }
    
    public string FirstUserUserName { get; init; } 
}