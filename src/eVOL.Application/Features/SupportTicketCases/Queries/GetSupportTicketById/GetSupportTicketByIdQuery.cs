using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById
{
    public record GetSupportTicketByIdQuery(int Id) : IRequest<SupportTicket?>; 
}
