using eVOL.Application.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage
{
    public class SendChatGroupMessageEventHandler : INotificationHandler<SendChatGroupMessageEvent>
    {

        private readonly IRabbitMqPublisher _publisher;
        private readonly ILogger<SendChatGroupMessageEventHandler> _logger;

        public SendChatGroupMessageEventHandler(IRabbitMqPublisher publisher, ILogger<SendChatGroupMessageEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Handle(SendChatGroupMessageEvent notification, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Sending message with id:{Id} to RabbitMq ", notification.Evt.MessageId);

            try
            {
                await _publisher.PublishAsync(notification.Evt);

                _logger.LogInformation("Successfullt sended message with id:{Id} to RabbitMq ", notification.Evt.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending message with id:{Id} to RabbitMq ", notification.Evt.MessageId);
            }
        }
    }
}
