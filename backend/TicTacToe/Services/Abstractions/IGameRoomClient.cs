using TicTacToe.Hubs;

namespace TicTacToe.Services.Abstractions;

public interface IGameRoomClient
{
    Task GameStarted(SignalRMessage info);
    Task GameEnded(SignalRMessage info);
    Task PlayerJoined(SignalRMessage info);
    Task PlayerLeft(SignalRMessage info);
    Task PlayerMadeTurn(SignalRMessage info);
}