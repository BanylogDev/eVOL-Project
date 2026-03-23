using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.LeaveChatGroup
{
    public record LeaveChatGroupCommand(Guid UserId, string ChatGroupName) : IRequest<User?>
    {
    }
}
