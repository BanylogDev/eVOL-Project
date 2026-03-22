using eVOL.Application.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public class SendSupportTicketMessageEventHandler : INotificationHandler<SendSupportTicketMessageEvent>
    {

        private readonly IRabbitMqPublisher _publisher;
        private readonly ILogger<SendSupportTicketMessageEventHandler> _logger;

        public SendSupportTicketMessageEventHandler(IRabbitMqPublisher publisher, ILogger<SendSupportTicketMessageEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Handle(SendSupportTicketMessageEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending message with id:{Id} to RabbitMq ", notification.NewMessage.MessageId);

            try
            {
                await _publisher.PublishAsync(notification.NewMessage);

                _logger.LogInformation("Successfullt sended message with id:{Id} to RabbitMq ", notification.NewMessage.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending message with id:{Id} to RabbitMq ", notification.NewMessage.MessageId);
            }
        }
    }
}
