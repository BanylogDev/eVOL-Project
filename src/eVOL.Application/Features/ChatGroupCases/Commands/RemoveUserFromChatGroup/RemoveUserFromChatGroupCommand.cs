using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup
{
    public record RemoveUserFromChatGroupCommand(Guid UserId, string ChatGroupName) : IRequest<User?>;

}
