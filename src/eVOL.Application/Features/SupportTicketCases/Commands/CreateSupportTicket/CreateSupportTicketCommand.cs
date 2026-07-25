
using eVOL.Application.DTOs.Requests.SupportTicketDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket
{
    public record CreateSupportTicketCommand(SupportTicketDto Dto, Guid UserId) : IRequest<ResultResponse>;
}
