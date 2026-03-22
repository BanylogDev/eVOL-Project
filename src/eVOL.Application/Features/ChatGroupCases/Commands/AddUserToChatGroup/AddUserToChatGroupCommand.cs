using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public record AddUserToChatGroupCommand(int UserId, string ChatGroupName) : IRequest<User?>;
}
