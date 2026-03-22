using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket
{
    public record CreateSupportTicketCommand(SupportTicketDTO Dto) : IRequest<SupportTicket?>;
}
