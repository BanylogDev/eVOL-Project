using eVOL.Application.DTOs.Responses.SupportTicketResponses.ApplicationLayer;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById
{
    public class GetSupportTicketByIdHandler : IRequestHandler<GetSupportTicketByIdQuery, GetSupportTicket>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<GetSupportTicketByIdHandler> _logger;

        public GetSupportTicketByIdHandler(IPostgreUnitOfWork uow, ILogger<GetSupportTicketByIdHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<GetSupportTicket> Handle(GetSupportTicketByIdQuery request, CancellationToken ct)
        {
            var supportTicket = await _uow.SupportTicket.GetSupportTicketById(request.Id, ct);

            if (supportTicket == null)
            {
                _logger.LogWarning("GetSupportTicketByIdUseCase: SupportTicket with ID {SupportTicketId} not found.", request.Id);
                return new GetSupportTicket
                {
                    IsSuccess = false,
                    Error = "Not Found."
                };
            }

            _logger.LogInformation("GetSupportTicketByIdUseCase: Successfully retrieved SupportTicket with ID {SupportTicketId}.", request.Id);

            return new GetSupportTicket
            {
                Name = supportTicket.Name,
                Category = supportTicket.Category,
                OpenedById = supportTicket.OpenedById,
                ClaimedById = supportTicket.ClaimedById,
                ClaimedStatus = supportTicket.ClaimedStatus,
                CreatedAt = supportTicket.CreatedAt,
                IsSuccess = true
            };
        }
    }
}
