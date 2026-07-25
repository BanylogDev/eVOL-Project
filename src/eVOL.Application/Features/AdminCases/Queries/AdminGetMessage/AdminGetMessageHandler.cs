using eVOL.Application.DTOs.Responses.Message;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetMessage
{
    public class AdminGetMessageHandler : IRequestHandler<AdminGetMessageQuery, ChatMessageResponse>
    {

        private readonly IMongoUnitOfWork _uow;
        private readonly ILogger<AdminGetMessageHandler> _logger;

        public AdminGetMessageHandler(IMongoUnitOfWork uow, ILogger<AdminGetMessageHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatMessageResponse> Handle(AdminGetMessageQuery request, CancellationToken ct)
        {

            _logger.LogInformation("AdminGetMessageHandler: Handling request for message ID {MessageId}", request.id);

            var chatMessage = await _uow.Message.GetChatMessageById(request.id, ct);

            if (chatMessage is null)
            {
                _logger.LogWarning("AdminGetMessageHandler: No message found for ID {MessageId}", request.id);
                return new ChatMessageResponse
                {
                    IsSuccess = false,
                    Error = $"No message found for ID {request.id}"
                };

            }

            _logger.LogInformation("AdminGetMessageHandler: Message found for ID {MessageId}", request.id);

            return new ChatMessageResponse
            {
                MessageId = chatMessage.MessageId,
                Text = chatMessage.Text,
                SenderId = chatMessage.SenderId,
                ReceiverId = chatMessage.ReceiverId,
                CreatedAt = chatMessage.CreatedAt,
                IsSuccess = true
            };
        }
    }
}
