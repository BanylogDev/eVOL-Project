using Asp.Versioning;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.UserCases.Commands.DeleteUser;
using eVOL.Application.Features.UserCases.Commands.UpdateUser;
using eVOL.Application.Features.UserCases.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVOL.API.Controllers.v2
{
    [ApiController]
    [Route("api/{version:apiVersion}/user")]
    [ApiVersion("2.0")]
    [Authorize(Roles = "User,Admin")]
    public class UserController : ControllerBase
    {

        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _sender.Send(new UpdateUserCommand(dto));

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAccountDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _sender.Send(new DeleteUserCommand(dto));

            if (user == null) return NotFound(new { message = $"User with id {dto.Id} not found" });

            return Ok(new { message = "Account has been deleted successfully", user.Name });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _sender.Send(new GetUserQuery(id));

            if (user == null) return NotFound("User wasnt found");

            return Ok(user);
        }
    }
}
