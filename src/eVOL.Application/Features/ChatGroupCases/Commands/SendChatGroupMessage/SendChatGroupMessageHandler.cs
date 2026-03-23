using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage
{
    public class SendChatGroupMessageHandler : IRequestHandler<SendChatGroupMessageCommand, (ChatMessage?, User?)>
    {

        private readonly IPublisher _publisher;
        private readonly IPostgreUnitOfWork _mysqluow;
        private readonly ILogger<SendChatGroupMessageHandler> _logger;

        public SendChatGroupMessageHandler(IPublisher publisher, IPostgreUnitOfWork mysqluow, ILogger<SendChatGroupMessageHandler> logger)
        {
            _publisher = publisher;
            _mysqluow = mysqluow;
            _logger = logger;
        }

        public async Task<(ChatMessage?, User?)> Handle(SendChatGroupMessageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started sending message from user with id: {UserId} to chat group with name: {ChatGroupName}, Text: {Text}", request.UserId, request.ChatGroupName, request.Message);

            await _mysqluow.BeginTransactionAsync();

            try
            {
                var chatGroup = await _mysqluow.ChatGroup.GetChatGroupByName(request.ChatGroupName);
                var user = await _mysqluow.Users.GetUserById(request.UserId);

                if (chatGroup == null || user == null) return (null, null);

                var evt = new ChatMessage
                {
                    MessageId = Guid.NewGuid(),
                    Text = request.Message,
                    SenderId = user.UserId,
                    ReceiverId = chatGroup.Id,
                    CreatedAt = DateTime.UtcNow
                };

                await _mysqluow.CommitAsync();

                await _publisher.Publish(new SendChatGroupMessageEvent(evt));

                return (new ChatMessage { Text = evt.Text, SenderId = evt.SenderId, ReceiverId = evt.ReceiverId, CreatedAt = evt.CreatedAt }, user);
            }
            catch (Exception ex)
            {
                await _mysqluow.RollbackAsync();
                _logger.LogError(ex, "Error, Something went wrong while sending message to chat group with name: {ChatGroupName}", request.ChatGroupName);
                throw;
            }
        }
    }
}
