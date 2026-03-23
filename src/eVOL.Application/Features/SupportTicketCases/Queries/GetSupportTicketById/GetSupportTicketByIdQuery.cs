using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById
{
    public record GetSupportTicketByIdQuery(Guid Id) : IRequest<SupportTicket?>; 
}
