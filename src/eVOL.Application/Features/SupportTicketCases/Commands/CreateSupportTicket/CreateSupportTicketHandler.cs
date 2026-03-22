using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket
{
    public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketCommand, SupportTicket?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<CreateSupportTicketHandler> _logger;

        public CreateSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<CreateSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<SupportTicket?> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting CreateSupportTicketUseCase for User ID: {UserId}", request.Dto.OpenedBy);

            await _uow.BeginTransactionAsync();

            try
            {

                var user = await _uow.Users.GetUserById(request.Dto.OpenedBy);

                if (user == null)
                {
                    return null;
                }

                var newSupportTicket = new SupportTicket()
                {
                    Category = request.Dto.Category,
                    Text = request.Dto.Text,
                    OpenedById = request.Dto.OpenedBy,
                    ClaimedById = 0,
                    OpenedBy = user,
                    CreatedAt = DateTime.UtcNow

                };

                _logger.LogInformation("Creating SupportTicket for User ID: {UserId}", request.Dto.OpenedBy);

                await _uow.SupportTicket.CreateSupportTicket(newSupportTicket);
                await _uow.CommitAsync();

                _logger.LogInformation("CreateSupportTicketUseCase completed successfully for User ID: {UserId}", request.Dto.OpenedBy);

                return newSupportTicket;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "CreateSupportTicketUseCase failed and rolled back for User ID: {UserId}", request.Dto.OpenedBy);
                throw;
            }
        }
    }
}
