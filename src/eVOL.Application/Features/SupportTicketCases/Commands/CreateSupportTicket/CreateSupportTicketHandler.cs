using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket
{
    public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<CreateSupportTicketHandler> _logger;

        public CreateSupportTicketHandler(IPostgreUnitOfWork uow, ILogger<CreateSupportTicketHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(CreateSupportTicketCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting CreateSupportTicketUseCase for User ID: {UserId}", request.UserId);

            var user = await _uow.Users.CheckUserExistance(request.UserId, ct);

            if (!user)
            {
                _logger.LogError("User not found in CreateSupportTicketUseCase with id: {UserId}", request.UserId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            var supportTicketId = Guid.NewGuid();

            var chatMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                Text = request.Dto.Text,
                SenderId = request.UserId,
                ReceiverId = supportTicketId,
                CreatedAt = DateTime.UtcNow,
            };

            var messagesList = new List<ChatMessage>();
            messagesList.Add(chatMessage);

            var newSupportTicket = new SupportTicket()
            {
                Id = supportTicketId,
                Category = request.Dto.Category,
                Messages = messagesList,
                OpenedById = request.UserId,
                CreatedAt = DateTime.UtcNow

            };

            _logger.LogInformation("Creating SupportTicket for User ID: {UserId}", request.UserId);

            if (!await _uow.SupportTicket.CreateSupportTicket(newSupportTicket, ct))
            {
                _logger.LogWarning("CreateSupportTicketUseCase failed.");
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Support Ticket Creation Failed."
                };
            }

            _logger.LogInformation("CreateSupportTicketUseCase completed successfully for User ID: {UserId}", request.UserId);

            return new ResultResponse
            {
                IsSuccess = true
            };

        }
    }
}
