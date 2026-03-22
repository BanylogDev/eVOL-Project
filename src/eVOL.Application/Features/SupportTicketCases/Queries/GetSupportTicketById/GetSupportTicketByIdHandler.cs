using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById
{
    public class GetSupportTicketByIdHandler : IRequestHandler<GetSupportTicketByIdQuery, SupportTicket?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<GetSupportTicketByIdHandler> _logger;

        public GetSupportTicketByIdHandler(IPostgreUnitOfWork uow, ILogger<GetSupportTicketByIdHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<SupportTicket?> Handle(GetSupportTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var supportTicket = await _uow.SupportTicket.GetSupportTicketById(request.Id);

            if (supportTicket == null)
            {
                _logger.LogWarning("GetSupportTicketByIdUseCase: SupportTicket with ID {SupportTicketId} not found.", request.Id);
                return null;
            }

            _logger.LogInformation("GetSupportTicketByIdUseCase: Successfully retrieved SupportTicket with ID {SupportTicketId}.", request.Id);

            return supportTicket;
        }
    }
}
