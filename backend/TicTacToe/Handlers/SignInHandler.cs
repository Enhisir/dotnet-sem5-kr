using TicTacToe.Common.CQRS;
using TicTacToe.Common.Helpers.Abstractions;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories;
using TicTacToe.Requests;
using TicTacToe.Services.Abstractions;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Handlers;

public class SignInHandler(
    IRepository<User> userRepository,
    IPasswordHasherService passwordHasherService,
    IAuthService authService
    ) : IHandler<SignInRequest, IResult>
{
    public async Task<IResult> Execute(
        SignInRequest request, 
        CancellationToken cancellationToken)
    {
        request.UserName = request.UserName.ToLower();
        var user = await userRepository
            .GetSingleOrDefault(u => u.UserName.Equals(request.UserName));
        if (user is null
            || !passwordHasherService
                .Validate(user.PasswordHashed, request.Password))
            return Results.Unauthorized();

        var response = authService.CreateResponse(user);
        return Results.Ok(response);
    }
}