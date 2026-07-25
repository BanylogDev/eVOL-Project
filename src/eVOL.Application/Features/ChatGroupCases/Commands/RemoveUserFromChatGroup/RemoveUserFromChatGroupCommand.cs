using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup
{
    public record RemoveUserFromChatGroupCommand(Guid OwnerId, Guid UserId, string ChatGroupName) : IRequest<ChatGroupResponse>;

}
