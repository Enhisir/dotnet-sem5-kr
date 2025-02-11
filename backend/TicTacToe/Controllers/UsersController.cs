using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;

namespace TicTacToe.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("rating")]
    [Authorize]
    public async Task<IResult> GetRating()
        => await mediator.Send(new GetRatingRequest(User.Identity!.Name!));
}
