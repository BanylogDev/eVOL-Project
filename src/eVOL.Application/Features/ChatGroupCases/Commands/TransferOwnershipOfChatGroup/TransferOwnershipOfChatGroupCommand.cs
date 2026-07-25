using eVOL.Application.DTOs.Requests.ChatGroupDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup
{
    public record TransferOwnershipOfChatGroupCommand(TransferOwnershipOfCG Dto, Guid CurrentOwnerId) : IRequest<ResultResponse>;

}
