using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup
{
    public record CreateChatGroupCommand(ChatGroupDTO Dto) : IRequest<ChatGroup>;
}
