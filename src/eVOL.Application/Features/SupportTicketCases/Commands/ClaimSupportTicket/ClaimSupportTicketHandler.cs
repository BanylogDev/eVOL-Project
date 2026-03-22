using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket
{
    public class ClaimSupportTicketHandler : IRequestHandler<ClaimSupportTicketCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<ClaimSupportTicketHandler> _logger;

        public ClaimSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<ClaimSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(ClaimSupportTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting ClaimSupportTicketUseCase for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.Dto.OpenedBy);

                var supportTicket = await _uow.SupportTicket.GetSupportTicketById(request.Dto.Id);

                if (user == null || supportTicket == null || supportTicket.ClaimedStatus == true)
                {
                    _logger.LogWarning("ClaimSupportTicketUseCase failed: User or SupportTicket not found, or SupportTicket already claimed.");
                    return null;
                }

                _logger.LogInformation("Claiming SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);
                supportTicket.ClaimedById = request.Dto.OpenedBy;
                supportTicket.ClaimedBy = user;
                supportTicket.ClaimedStatus = true;
                _logger.LogInformation("SupportTicket ID: {SupportTicketId} successfully claimed by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

                await _uow.CommitAsync();

                _logger.LogInformation("ClaimSupportTicketUseCase completed successfully for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "ClaimSupportTicketUseCase failed and rolled back for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);
                throw;
            }
        }
    }
}
