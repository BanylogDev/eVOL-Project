using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketHandler : IRequestHandler<DeleteSupportTicketCommand, SupportTicket?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<DeleteSupportTicketHandler> _logger;

        public DeleteSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<DeleteSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<SupportTicket?> Handle(DeleteSupportTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting DeleteSupportTicketUseCase for SupportTicket ID: {SupportTicketId}", request.Id);

            await _uow.BeginTransactionAsync();

            try
            {
                var supportTicket = await _uow.SupportTicket.GetSupportTicketById(request.Id);

                if (supportTicket == null)
                {
                    _logger.LogWarning("DeleteSupportTicketUseCase failed: SupportTicket not found.");
                    return null;
                }

                _logger.LogInformation("Deleting SupportTicket ID: {SupportTicketId}", request.Id);

                _uow.SupportTicket.DeleteSupportTicket(supportTicket);
                await _uow.CommitAsync();

                _logger.LogInformation("DeleteSupportTicketUseCase completed successfully for SupportTicket ID: {SupportTicketId}", request.Id);

                return supportTicket;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "DeleteSupportTicketUseCase failed and rolled back for SupportTicket ID: {SupportTicketId}", request.Id);
                throw;
            }
        }
    }
}
