using eVOL.Application.DTOs.Requests.SupportTicketDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket
{
    public record ClaimSupportTicketCommand(ClaimSupportTicketDTO Dto, Guid ClaimerId) : IRequest<ResultResponse>;
}
