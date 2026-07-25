using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket
{
    public record DeleteSupportTicketCommand(Guid Id) : IRequest<ResultResponse>;

}
