using Microsoft.AspNetCore.Mvc;
using TicTacToe.Common.CQRS;
using TicTacToe.Requests;

namespace TicTacToe.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("sign_in")]
    public async Task<IResult> SignIn([FromBody] SignInRequest request)
        => await mediator.Send(request);
    
    [HttpPost("sign_up")]
    public async Task<IResult> SignUp([FromBody] SignUpRequest request)
        => await mediator.Send(request);
    
    [HttpPost("refresh_token")]
    public async Task<IResult> Refresh([FromBody] RefreshRequest request)
        => await mediator.Send(request);
}