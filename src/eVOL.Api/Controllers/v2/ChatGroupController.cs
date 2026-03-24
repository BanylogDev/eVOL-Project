using Asp.Versioning;
using eVOL.Application.DTOs;
using eVOL.Application.DTOs.Requests;
using eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup;
using eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup;
using eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup;
using eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVOL.API.Controllers.v2
{
    [ApiController]
    [Route("api/{version:apiVersion}/chat-group")]
    [ApiVersion("2.0")]
    [Authorize(Roles = "User,Admin")]
    public class ChatGroupController : ControllerBase
    {
        private readonly ISender _sender;

        public ChatGroupController(ISender sender)
        {
            _sender = sender;
        }   

        [HttpPost("create")]
        public async Task<IActionResult> CreateChatGroup(ChatGroupDTO dto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var chatGroup = await _sender.Send(new CreateChatGroupCommand(dto));

            if (chatGroup == null) return BadRequest("Something went wrong.");

            return Ok(chatGroup);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteChatGroup([FromBody] DeleteChatGroupDTO dto)
        {
            var chatGroup = await _sender.Send(new DeleteChatGroupCommand(dto));

            if (chatGroup == null) return NotFound();

            return Ok(chatGroup);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetChatGroupById(Guid id)
        {
            var chatGroup = await _sender.Send(new GetChatGroupByIdQuery(id));

            if (chatGroup == null) return NotFound();

            return Ok(chatGroup);
        }

        [HttpPut("transfer")]
        public async Task<IActionResult> TransferOwnershipOfChatGroup(TransferOwnershipOfCGDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var chatGroup = await _sender.Send(new TransferOwnershipOfChatGroupCommand(dto));

            if (chatGroup == null) return NotFound("Something went wrong");

            return Ok(chatGroup);


        }

    }
}
