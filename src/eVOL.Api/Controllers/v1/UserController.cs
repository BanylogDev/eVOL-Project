using Asp.Versioning;
using eVOL.Application.DTOs.Requests.UserDTO;
using eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO;
using eVOL.Application.Features.UserCases.Commands.DeleteUser;
using eVOL.Application.Features.UserCases.Commands.UpdateUser;
using eVOL.Application.Features.UserCases.Commands.UpdateUserEmail;
using eVOL.Application.Features.UserCases.Commands.UpdateUserPassword;
using eVOL.Application.Features.UserCases.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace eVOL.API.Controllers.v1
{
    [ApiController]
    [Route("api/{version:apiVersion}/user")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "User,Admin")]
    public class UserController : ControllerBase
    {

        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPatch("edit/name")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateName dto, CancellationToken ct)
        {

            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new UpdateUserNameCommand(userId, dto), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("edit/email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmail dto, CancellationToken ct)
        {

            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new UpdateUserEmailCommand(userId, dto), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("edit/password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword dto, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new UpdateUserPasswordCommand(userId, dto), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAccount dto, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new DeleteUserCommand(userId, dto), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var user = await _sender.Send(new GetUserQuery(userId), ct);

            if (!user.IsSuccess) return NotFound(user);

            return Ok(user);
        }
    }
}
