using eVOL.Application.DTOs.Responses.SupportTicketResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById
{
    public record GetSupportTicketByIdQuery(Guid Id) : IRequest<GetSupportTicket>;
}
