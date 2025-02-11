using TicTatToe.Data.Enum;

namespace TicTacToe.Responses;

public record ViewGameRoomResponse(
    Guid Id, 
    DateTime CreatedAt,
    State State,
    List<string> Players);