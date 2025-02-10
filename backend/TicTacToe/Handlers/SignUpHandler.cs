using TicTacToe.Common.CQRS;
using TicTacToe.Common.Helpers.Abstractions;
using TicTacToe.Requests;
using TicTacToe.Services.Abstractions;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories;

namespace TicTacToe.Handlers;

public class SignUpHandler(
    IRepository<User> userRepository,
    IPasswordHasherService passwordHasherService,
    IAuthService authService
    ) : IHandler<SignUpRequest, IResult>
{
    public async Task<IResult> Execute(
        SignUpRequest request, 
        CancellationToken cancellationToken)
    {
        var maybeUser = await userRepository
            .GetSingleOrDefault(
                u => u.UserName.Equals(
                    request.UserName, 
                    StringComparison.OrdinalIgnoreCase));
        if (maybeUser is not null)
            return Results.BadRequest("Username already exists");

        var newUser = new User
        {
            UserName = request.UserName,
            PasswordHashed = passwordHasherService.Hash(request.Password)
        };
        var response = authService.CreateResponse(newUser);
        return Results.Ok(response);
    }
}