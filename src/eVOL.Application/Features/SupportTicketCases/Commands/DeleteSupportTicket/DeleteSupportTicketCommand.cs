using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket
{
    public record DeleteSupportTicketCommand(Guid Id) : IRequest<SupportTicket?>;

}
