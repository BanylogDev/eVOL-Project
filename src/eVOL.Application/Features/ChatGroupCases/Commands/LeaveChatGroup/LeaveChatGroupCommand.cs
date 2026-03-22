using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.LeaveChatGroup
{
    public record LeaveChatGroupCommand(int UserId, string ChatGroupName) : IRequest<User?>
    {
    }
}
