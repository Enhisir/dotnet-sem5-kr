using TicTatToe.Data.Enum;

namespace TicTacToe.Hubs;

public class GameRoomHubContextItemsHelper(IDictionary<object, object?> dictionary)
{
    private const string StatusItem = "Status";
    private const string GameRoomIdItem = "GameRoomId";
    private const string UserIdItem = "UserId";
    
    public Guid GameRoomId
    {
        get => (Guid)(dictionary[GameRoomIdItem] ?? Guid.Empty);
        set => dictionary[GameRoomIdItem] = value;
    }

    public string UserName
    {
        get => (string)(dictionary[UserIdItem] ?? string.Empty);
        set => dictionary[UserIdItem] = value;
    }
    
    public PlayerStatus Status
    {
        get => (PlayerStatus)(dictionary[StatusItem] ?? PlayerStatus.Observer);
        set => dictionary[StatusItem] = value;
    }
}