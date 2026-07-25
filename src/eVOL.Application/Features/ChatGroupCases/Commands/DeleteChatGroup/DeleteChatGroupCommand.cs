using eVOL.Application.DTOs.Requests.ChatGroupDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup
{
    public record DeleteChatGroupCommand(DeleteChatGroupDTO Dto, Guid UserId) : IRequest<ResultResponse>;
}
