using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket
{
    public class ClaimSupportTicketHandler : IRequestHandler<ClaimSupportTicketCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<ClaimSupportTicketHandler> _logger;

        public ClaimSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<ClaimSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(ClaimSupportTicketCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting ClaimSupportTicketUseCase for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.SupportTicketId, request.ClaimerId);

            if (!await _uow.SupportTicket.ClaimSupportTicket(request.Dto.SupportTicketId, request.ClaimerId, ct))
            {
                _logger.LogWarning("ClaimSupportTicketUseCase failed claiming SupportTicket with ID: {SupportTicketId}", request.Dto.SupportTicketId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Not Found."
                };
            }

            _logger.LogInformation("ClaimSupportTicketUseCase completed successfully for SupportTicket ID: {SupportTicketId} by User ID: {UserId}", request.Dto.SupportTicketId, request.ClaimerId);

            return new ResultResponse
            {
                IsSuccess = true,
            };


        }
    }
}
