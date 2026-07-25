using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.LeaveChatGroup
{
    public record LeaveChatGroupCommand(Guid UserId, string ChatGroupName) : IRequest<ChatGroupResponse>
    {
    }
}
