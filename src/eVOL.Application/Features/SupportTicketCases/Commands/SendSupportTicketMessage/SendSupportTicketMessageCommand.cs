using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public record SendSupportTicketMessageCommand(string Message, string SupportTicketName, Guid UserId) : IRequest<(ChatMessage?, User?)>;
}
