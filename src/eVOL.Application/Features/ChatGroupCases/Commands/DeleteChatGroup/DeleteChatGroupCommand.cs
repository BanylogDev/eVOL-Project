using eVOL.Application.DTOs;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup
{
    public record DeleteChatGroupCommand(DeleteChatGroupDTO Dto) : IRequest<ChatGroup?>;
}
