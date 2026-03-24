using Asp.Versioning;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket;
using eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVOL.API.Controllers.v2
{
    [ApiController]
    [Route("api/{version:apiVersion}/support-ticket")]
    [ApiVersion("2.0")]
    [Authorize(Roles = "User,Admin")]
    public class SupportTicketController : ControllerBase
    {
        private readonly ISender _sender;

        public SupportTicketController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        public async Task<IActionResult> CreateSupportTicket(SupportTicketDTO dto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var supportTicket = await _sender.Send(new CreateSupportTicketCommand(dto));

            if (supportTicket == null) return BadRequest("Something went wrong");

            return Ok(supportTicket);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSupportTicket(Guid id)
        {
            var supportTicket = await _sender.Send(new DeleteSupportTicketCommand(id));

            if (supportTicket == null) return NotFound("Something went wrong");

            return Ok(supportTicket);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSupportTicketById(Guid id)
        {
            var supportTicket = await _sender.Send(new GetSupportTicketByIdQuery(id));

            if (supportTicket == null) return NotFound("Support ticket wasnt found");

            return Ok(supportTicket);
        }

        [HttpPost("claim")]
        public async Task<IActionResult> ClaimSupportTicket([FromBody] ClaimSupportTicketDTO dto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _sender.Send(new ClaimSupportTicketCommand(dto));

            if (user == null) return NotFound("Something went wrong");

            return Ok(user);
        }

        [HttpDelete("unclaim")]
        public async Task<IActionResult> UnClaimSupportTicket([FromBody] ClaimSupportTicketDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _sender.Send(new UnClaimSupportTicketCommand(dto)); 

            if (user == null) return NotFound("Something went wrong");

            return Ok(user);
        }
    }
}
