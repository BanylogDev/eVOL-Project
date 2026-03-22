using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public record SendSupportTicketMessageEvent(ChatMessage NewMessage) : INotification;
}
