using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage
{
    public record SendChatGroupMessageCommand(string Message, string ChatGroupName, Guid UserId) : IRequest<ResultResponse>;
}
