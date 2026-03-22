using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup
{
    public record RemoveUserFromChatGroupCommand(int UserId, string ChatGroupName) : IRequest<User?>;

}
