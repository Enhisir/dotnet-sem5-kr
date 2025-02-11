using TicTatToe.Data.Enum;

namespace TicTacToe.Responses;

public class ViewGameRoomResponse(
    Guid Id, 
    DateTime CreatedAt,
    State State,
    List<string> Players);