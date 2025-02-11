using Microsoft.EntityFrameworkCore;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;
using TicTatToe.Data.Enum;
using TicTatToe.Data.Models;
using TicTatToe.Data.Repositories;

namespace TicTacToe.Handlers;

public class ViewGameRoomListHandler(
    IRepository<GameRoom> gameRoomRepository
) : IHandler<ViewGameRoomListRequest, IResult>
{
    public async Task<IResult> Execute(ViewGameRoomListRequest request, CancellationToken cancellationToken)
        => Results.Ok(
            await gameRoomRepository
                .GetRange()
                .OrderBy(gr => gr.State)
                .Where(gr => gr.State != State.Closed)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(cancellationToken: cancellationToken));
}