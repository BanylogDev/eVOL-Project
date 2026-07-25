using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public record AddUserToChatGroupCommand(Guid OwnerId, Guid UserId, string ChatGroupName) : IRequest<ChatGroupResponse>;
}
