using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public class SendSupportTicketMessageHandler : IRequestHandler<SendSupportTicketMessageCommand, ResultResponse>
    {

        private readonly IPublisher _publisher;
        private readonly IPostgreUnitOfWork _mysqluow;
        private readonly ILogger<SendSupportTicketMessageHandler> _logger;

        public SendSupportTicketMessageHandler(IPublisher publisher, IPostgreUnitOfWork mysqluow, ILogger<SendSupportTicketMessageHandler> logger)
        {
            _publisher = publisher;
            _mysqluow = mysqluow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(SendSupportTicketMessageCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started sending message from user with id: {UserId} to support ticket with name: {SupportTicketName}, Text: {Text}", request.UserId, request.SupportTicketName, request.Message);

            if (!await _mysqluow.Users.CheckUserExistance(request.UserId, ct))
            {
                _logger.LogWarning("User with id: {UserId} not found", request.UserId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            var supportTicket = await _mysqluow.SupportTicket.GetSupportTicketByName(request.SupportTicketName, ct);

            if (supportTicket == null)
            {
                _logger.LogWarning("Support ticket with name: {SupportTicketName} or user with id: {UserId} weren't found!", request.SupportTicketName, request.UserId);
                return new ResultResponse
                {
                    IsSuccess = false
                };
            }

            var newMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                Text = request.Message,
                SenderId = request.UserId,
                ReceiverId = supportTicket.SupportTicketId,
                CreatedAt = DateTime.UtcNow,
            };

            await _publisher.Publish(new SendSupportTicketMessageEvent(newMessage));

            _logger.LogInformation("Ended sending message from user with id: {UserId} to support ticket with name: {SupportTicketName}, Text: {Text}, Success!", request.UserId, request.SupportTicketName, request.Message);

            return new ResultResponse
            {
                IsSuccess = true
            };

        }
    }
}
