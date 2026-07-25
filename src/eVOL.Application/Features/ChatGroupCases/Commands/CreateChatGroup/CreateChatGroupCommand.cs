using eVOL.Application.DTOs.Requests.ChatGroupDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup
{
    public record CreateChatGroupCommand(ChatGroupCreate Dto, Guid Id) : IRequest<ResultResponse>;
}
