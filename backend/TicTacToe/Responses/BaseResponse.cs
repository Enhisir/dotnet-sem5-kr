namespace TicTacToe.Responses;

public record BaseResponse(bool IsSuccess, string? Message = null)
{
    public static BaseResponse Success { get; } = new(true);
    public static BaseResponse Failure(string? msg = null) => new(false, msg);
}