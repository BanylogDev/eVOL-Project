using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public record AddUserToChatGroupCommand(Guid UserId, string ChatGroupName) : IRequest<User?>;
}
