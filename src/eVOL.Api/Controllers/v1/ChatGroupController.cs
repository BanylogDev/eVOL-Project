using Asp.Versioning;
using eVOL.Application.DTOs.Requests.ChatGroupDTO;
using eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup;
using eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup;
using eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup;
using eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace eVOL.API.Controllers.v1
{
    [ApiController]
    [Route("api/{version:apiVersion}/chat-group")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "User,Admin")]
    public class ChatGroupController : ControllerBase
    {
        private readonly ISender _sender;

        public ChatGroupController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChatGroup(ChatGroupCreate dto)
        {

            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new CreateChatGroupCommand(dto, userId));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteChatGroup([FromBody] DeleteChatGroupDTO dto)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new DeleteChatGroupCommand(dto, userId));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetChatGroupById(Guid id)
        {
            var chatGroup = await _sender.Send(new GetChatGroupByIdQuery(id));

            if (chatGroup == null) return NotFound();

            return Ok(chatGroup);
        }

        [HttpPut("transfer")]
        public async Task<IActionResult> TransferOwnershipOfChatGroup(TransferOwnershipOfCG dto)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

            var result = await _sender.Send(new TransferOwnershipOfChatGroupCommand(dto, userId));

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);


        }

    }
}
