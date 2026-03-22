using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;
namespace eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket
{
    public record ClaimSupportTicketCommand(ClaimSupportTicketDTO Dto) : IRequest<User?>;
}
