using Asp.Versioning;
using eVOL.Application.DTOs.Requests.Admin;
using eVOL.Application.Features.AdminCases.Commands.AdminBanUser;
using eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser;
using eVOL.Application.Features.AdminCases.Commands.AdminUnBanUser;
using eVOL.Application.Features.AdminCases.Queries.AdminGetMessage;
using eVOL.Application.Features.AdminCases.Queries.AdminGetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace eVOL.API.Controllers.v1
{
    [ApiController]
    [Route("api/{version:apiVersion}/admin")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {

        private readonly ISender _sender;
        public AdminController(ISender sender) => _sender = sender;


        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetUserInfo(Guid id, CancellationToken ct)
        {
            var result = await _sender.Send(new AdminGetUserQuery(id), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("message/{id:guid}")]
        public async Task<IActionResult> GetMessageInfo(Guid id, CancellationToken ct)
        {
            var result = await _sender.Send(new AdminGetMessageQuery(id), ct);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("user/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _sender.Send(new AdminDeleteUserCommand(id));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("user/ban/{id:guid}")]
        public async Task<IActionResult> BanUser(Ban dto)
        {

            var adminId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new AdminBanUserCommand(dto, adminId));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("user/unban/{id:guid}")]
        public async Task<IActionResult> UnBanUser(Guid id)
        {
            var result = await _sender.Send(new AdminUnBanUserCommand(id));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }
    }
}
