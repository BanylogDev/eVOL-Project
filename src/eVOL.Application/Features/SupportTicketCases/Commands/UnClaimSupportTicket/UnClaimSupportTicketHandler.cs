using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket
{
    public class UnClaimSupportTicketHandler : IRequestHandler<UnClaimSupportTicketCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<UnClaimSupportTicketHandler> _logger;

        public UnClaimSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<UnClaimSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(UnClaimSupportTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting UnClaimSupportTicketUseCase for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.Dto.OpenedBy);

                var supportTicket = await _uow.SupportTicket.GetSupportTicketById(request.Dto.Id);

                if (user == null || supportTicket == null || supportTicket.ClaimedStatus == false)
                {
                    _logger.LogWarning("UnClaimSupportTicketUseCase failed: User or SupportTicket not found, or SupportTicket is not claimed.");
                    return null;
                }

                _logger.LogInformation("Unclaiming SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

                supportTicket.ClaimedById = Guid.Parse("00000000-0000-0000-0000-000000000000");
                supportTicket.ClaimedBy = user;
                supportTicket.ClaimedStatus = false;

                _logger.LogInformation("SupportTicket ID: {SupportTicketId} successfully unclaimed by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

                await _uow.CommitAsync();

                _logger.LogInformation("UnClaimSupportTicketUseCase completed successfully for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "UnClaimSupportTicketUseCase failed and rolled back for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.Id, request.Dto.OpenedBy);
                throw;
            }
        }
    }
}
