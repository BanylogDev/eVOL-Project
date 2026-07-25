using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketHandler : IRequestHandler<DeleteSupportTicketCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<DeleteSupportTicketHandler> _logger;

        public DeleteSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<DeleteSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(DeleteSupportTicketCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting DeleteSupportTicketUseCase for SupportTicket ID: {SupportTicketId}", request.Id);

            if (!await _uow.SupportTicket.DeleteSupportTicket(request.Id, ct))
            {
                _logger.LogError("DeleteSupportTicketUseCase failed for SupportTicket ID: {SupportTicketId}", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Not Found."
                };
            }

            _logger.LogInformation("DeleteSupportTicketUseCase completed successfully for SupportTicket ID: {SupportTicketId}", request.Id);

            return new ResultResponse
            {
                IsSuccess = true,
            };


        }
    }
}
