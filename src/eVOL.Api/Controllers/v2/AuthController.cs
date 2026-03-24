using Asp.Versioning;
using eVOL.Application.DTOs;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.UserCases.Commands.LoginUser;
using eVOL.Application.Features.UserCases.Commands.RefreshToken;
using eVOL.Application.Features.UserCases.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eVOL.API.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("2.0")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _sender.Send(new RegisterUserCommand(dto));

            if (user == null) return BadRequest("Something went wrong");

            return Ok(new { message = "Registration successful", user.Name, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _sender.Send(new LoginUserCommand(dto));

            if (user == null) return Unauthorized("Invalid username or password");

            return Ok(new { message = "Login successful", user.UserId, user.Name, user.Email, user.AccessToken, user.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDto)
        {
            if (tokenDto is null) return BadRequest("Invalid client request");

            var tokens = await _sender.Send(new RefreshTokenCommand(tokenDto));

            return Ok(new
            {
                token = tokens?.AccessToken,
                refreshToken = tokens?.RefreshToken
            });
        }


    }
}
