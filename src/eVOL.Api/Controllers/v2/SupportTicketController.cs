using Asp.Versioning;
using eVOL.Application.DTOs.Requests.SupportTicketDTO;
using eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace eVOL.API.Controllers.v2
{
    [ApiController]
    [Route("api/{version:apiVersion}/support-ticket")]
    [ApiVersion("2.0")]
    [Authorize(Roles = "User,Admin,Support")]
    public class SupportTicketController : ControllerBase
    {
        private readonly ISender _sender;

        public SupportTicketController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        public async Task<IActionResult> CreateSupportTicket(SupportTicketDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new CreateSupportTicketCommand(dto, userId));

            if (!result.IsSuccess) return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> DeleteSupportTicket(Guid id)
        {
            var result = await _sender.Send(new DeleteSupportTicketCommand(id));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSupportTicketById(Guid id)
        {
            var result = await _sender.Send(new GetSupportTicketByIdQuery(id));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpPost("claim")]
        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> ClaimSupportTicket([FromBody] ClaimSupportTicketDTO dto)
        {

            var claimerId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new ClaimSupportTicketCommand(dto, claimerId));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("unclaim")]
        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> UnClaimSupportTicket([FromBody] ClaimSupportTicketDTO dto)
        {
            var result = await _sender.Send(new UnClaimSupportTicketCommand(dto));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }
    }
}
