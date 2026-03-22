using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket
{
    public record UnClaimSupportTicketCommand(ClaimSupportTicketDTO Dto) : IRequest<User?>;
}
