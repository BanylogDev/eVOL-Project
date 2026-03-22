using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage
{
    public record SendChatGroupMessageEvent(ChatMessage Evt) : INotification;
}
