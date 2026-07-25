using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public record SendSupportTicketMessageCommand(string Message, string SupportTicketName, Guid UserId) : IRequest<ResultResponse>;
}
