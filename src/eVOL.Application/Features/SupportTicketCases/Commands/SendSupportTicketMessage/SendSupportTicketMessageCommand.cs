using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public record SendSupportTicketMessageCommand(string Message, string SupportTicketName, int UserId) : IRequest<(ChatMessage?, User?)>;
}
