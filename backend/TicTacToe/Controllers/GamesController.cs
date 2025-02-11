using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Common.CQRS;
using TicTacToe.Dtos;
using TicTacToe.Requests;

namespace TicTacToe.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GamesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> ViewGameRoomList([FromQuery] int offset, [FromQuery] int limit)
        => await mediator.Send(new ViewGameRoomListRequest(offset, limit));
    
    [HttpPost("create")]
    public async Task<IResult> CreateGame(CreateGameDto gameDto)
        => await mediator.Send(new CreateGameRequest(User.Identity!.Name!, gameDto.MaxRating));
    
    // [HttpPost("enter")]
    // public async Task<IResult> EnterGameRoom(EnterGameRoomDto gameDto)
    //     => await mediator.Send(new EnterGameRoomRequest(gameDto.GameRoomId, User.Identity!.Name!));
    
    // [HttpPost("leave")]
    // public async Task<IResult> LeaveGameRoom(LeaveGameDto gameDto)
    //     => await mediator.Send(new LeaveGameRoomRequest(gameDto.GameRoomId, User.Identity!.Name!));
}