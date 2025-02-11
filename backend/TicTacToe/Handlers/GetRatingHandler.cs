using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTatToe.Data.Models;
using TicTatToe.Data.Storage;

namespace TicTacToe.Handlers;

public class GetRatingHandler(
    MongoStorage<Rating> ratingMongoStorage
) : IHandler<GetRatingRequest, IResult>
{
    public async Task<IResult> Execute(
        GetRatingRequest request, 
        CancellationToken cancellationToken)
    {
        var rating = 
            await ratingMongoStorage.GetOneAsync(
                r => r.UserName.Equals(
                    request.UserName,
                    StringComparison.OrdinalIgnoreCase));
        return rating is null ? Results.NotFound() : Results.Ok(rating);
    }
}