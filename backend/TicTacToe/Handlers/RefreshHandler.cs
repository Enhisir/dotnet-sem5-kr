using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTacToe.Services.Abstractions;

namespace TicTacToe.Handlers;

public class RefreshHandler(
    IAuthService authService
) : IHandler<RefreshRequest, IResult>
{
    public async Task<IResult> Execute(
        RefreshRequest request, 
        CancellationToken cancellationToken)
    {
        var response = authService.Refresh(request.RefreshToken);
        if (response is not null)
            return await Task.FromResult(Results.Ok(response));
        
        return await Task.FromResult(Results.Unauthorized());
    }
}