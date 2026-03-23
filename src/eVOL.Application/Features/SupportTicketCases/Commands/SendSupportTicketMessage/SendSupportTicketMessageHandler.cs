using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public class SendSupportTicketMessageHandler : IRequestHandler<SendSupportTicketMessageCommand, (ChatMessage?, User?)>
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

        public async Task<(ChatMessage?, User?)> Handle(SendSupportTicketMessageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started sending message from user with id: {UserId} to support ticket with name: {SupportTicketName}, Text: {Text}", request.UserId, request.SupportTicketName, request.Message);

            await _mysqluow.BeginTransactionAsync();

            try
            {

                var supportTicket = await _mysqluow.SupportTicket.GetSupportTicketByName(request.SupportTicketName);

                var user = await _mysqluow.Users.GetUserById(request.UserId);

                if (supportTicket == null || user == null)
                {
                    _logger.LogWarning("Support ticket with name: {SupportTicketName} or user with id: {UserId} weren't found!", request.SupportTicketName, request.UserId);
                    return (null, null);
                }

                var newMessage = new ChatMessage
                {
                    MessageId = Guid.NewGuid(),
                    Text = request.Message,
                    SenderId = user.UserId,
                    ReceiverId = supportTicket.Id,
                    CreatedAt = DateTime.UtcNow,
                };

                _logger.LogInformation("Adding custom message to mongo database!");

                await _mysqluow.CommitAsync();

                _logger.LogInformation("Finished adding custom message to mongo database!");

                await _publisher.Publish(new SendSupportTicketMessageEvent(newMessage));

                _logger.LogInformation("Ended sending message from user with id: {UserId} to support ticket with name: {SupportTicketName}, Text: {Text}, Success!", request.UserId, request.SupportTicketName, request.Message);

                return (newMessage, user);
            }
            catch (Exception ex)
            {
                await _mysqluow.RollbackAsync();
                _logger.LogError(ex, "Error, Something went wrong while sending message to support ticket with name: {SupportTicketName}", request.SupportTicketName);
                throw;
            }
        }
    }
}
