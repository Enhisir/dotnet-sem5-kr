using TicTacToe.Common.CQRS;
using TicTacToe.Common.Helpers.Abstractions;
using TicTacToe.Requests;
using TicTacToe.Services.Abstractions;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;
using TicTatToe.Data.Storage;

namespace TicTacToe.Handlers;

public class SignUpHandler(
    IRepository<User> userRepository,
    MongoStorage<Rating> ratingStorage,
    IPasswordHasherService passwordHasherService,
    IAuthService authService
    ) : IHandler<SignUpRequest, IResult>
{
    public async Task<IResult> Execute(
        SignUpRequest request, 
        CancellationToken cancellationToken)
    {
        request.UserName = request.UserName.ToLower();
        var maybeUser = await userRepository
            .GetSingleOrDefault(u => u.UserName.Equals(request.UserName));
        if (maybeUser is not null)
            return Results.BadRequest("Username already exists");

        var newUser = new User
        {
            UserName = request.UserName,
            PasswordHashed = passwordHasherService.Hash(request.Password)
        };
        await userRepository.AddAsync(newUser);
        await ratingStorage.CreateAsync(new Rating { UserName = newUser.UserName, Value = 0 });
        var response = authService.CreateResponse(newUser);
        return Results.Ok(response);
    }
}