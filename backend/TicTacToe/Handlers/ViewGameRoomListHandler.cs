using Microsoft.EntityFrameworkCore;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTacToe.Responses;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories.Abstractions;

namespace TicTacToe.Handlers;

public class ViewGameRoomListHandler(
    IRepository<GameRoom> gameRoomRepository
) : IHandler<ViewGameRoomListRequest, IResult>
{
    public async Task<IResult> Execute(ViewGameRoomListRequest request, CancellationToken cancellationToken)
    {
        var gamerooms = await gameRoomRepository
            .GetRange()
            .OrderBy(gr => gr.State)
            .Where(gr => gr.State != State.Closed)
            .Skip(request.Offset)
            .Take(request.Limit)
            .Include(gr => gr.Players)
            .ToListAsync(cancellationToken: cancellationToken);

        var response = 
            gamerooms
            .Select(gr => new ViewGameRoomResponse(gr.Id, gr.Players!.Select(p => p.UserName).ToList()))
            .ToList();
        return Results.Ok(response);
    }
}