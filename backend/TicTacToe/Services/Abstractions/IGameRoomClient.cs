using TicTacToe.Responses;

namespace TicTacToe.Services.Abstractions;

public interface IGameRoomClient
{
    Task GameStarted(GameRoomResponse info);
    Task GameEnded(GameRoomResponse info);
    Task PlayerJoined(GameRoomResponse info);
    Task PlayerLeft(GameRoomResponse info);
    Task PlayerMadeTurn(PlayerMadeTurnResponse info);
}