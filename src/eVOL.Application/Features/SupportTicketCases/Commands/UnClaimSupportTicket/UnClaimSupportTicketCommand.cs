using eVOL.Application.DTOs.Requests.SupportTicketDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket
{
    public record UnClaimSupportTicketCommand(ClaimSupportTicketDTO Dto) : IRequest<ResultResponse>;
}
