using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket
{
    public class UnClaimSupportTicketHandler : IRequestHandler<UnClaimSupportTicketCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<UnClaimSupportTicketHandler> _logger;

        public UnClaimSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<UnClaimSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(UnClaimSupportTicketCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting UnClaimSupportTicketUseCase for SupportTicket ID: {SupportTicketId}", request.Dto.SupportTicketId);

            if (!await _uow.SupportTicket.UnClaimSupportTicket(request.Dto.SupportTicketId, ct))
            {
                _logger.LogError("UnClaimSupportTicketUseCase failed and rolled back for SupportTicket ID: {SupportTicketId}", request.Dto.SupportTicketId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Not Found."
                };
            }

            _logger.LogInformation("UnClaimSupportTicketUseCase completed successfully for SupportTicket ID: {SupportTicketId}", request.Dto.SupportTicketId);

            return new ResultResponse
            {
                IsSuccess = true,
            };

        }
    }
}
