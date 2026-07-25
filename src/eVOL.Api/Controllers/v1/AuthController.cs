using Asp.Versioning;
using eVOL.Application.DTOs.Requests.UserDTO;
using eVOL.Application.Features.UserCases.Commands.LoginUser;
using eVOL.Application.Features.UserCases.Commands.RefreshToken;
using eVOL.Application.Features.UserCases.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eVOL.API.Controllers.v1
{
    [ApiController]
    [Route("api/{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register dto, CancellationToken ct)
        {
            var result = await _sender.Send(new RegisterUserCommand(dto), ct);

            if (!result.IsSuccess) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login dto, CancellationToken ct)
        {
            var result = await _sender.Send(new LoginUserCommand(dto), ct);

            if (!result.IsSuccess) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token tokenDto, CancellationToken ct)
        {
            var result = await _sender.Send(new RefreshTokenCommand(tokenDto), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }


    }
}
