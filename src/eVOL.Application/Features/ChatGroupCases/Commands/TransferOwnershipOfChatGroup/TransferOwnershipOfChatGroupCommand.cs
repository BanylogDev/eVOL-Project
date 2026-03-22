using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup
{
    public record TransferOwnershipOfChatGroupCommand(TransferOwnershipOfCGDTO Dto) : IRequest<ChatGroup?>;

}
