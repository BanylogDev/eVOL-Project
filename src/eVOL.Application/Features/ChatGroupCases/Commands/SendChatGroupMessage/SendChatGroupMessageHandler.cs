using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage
{
    public class SendChatGroupMessageHandler : IRequestHandler<SendChatGroupMessageCommand, ResultResponse>
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

        public async Task<ResultResponse> Handle(SendChatGroupMessageCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started sending message from user with id: {UserId} to chat group with name: {ChatGroupName}, Text: {Text}", request.UserId, request.ChatGroupName, request.Message);

            if (!await _mysqluow.Users.CheckUserExistance(request.UserId, ct))
            {
                _logger.LogWarning("User with id: {UserId} not found", request.UserId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            var chatGroup = await _mysqluow.ChatGroup.GetChatGroupIdByName(request.ChatGroupName, ct);

            if (chatGroup == null)
            {
                _logger.LogWarning("Chat Group with name: {ChatGroupName} wasnt found.", request.ChatGroupName);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Chat group not found."
                };
            }

            var chatMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                Text = request.Message,
                SenderId = request.UserId,
                ReceiverId = chatGroup.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _publisher.Publish(new SendChatGroupMessageEvent(chatMessage));

            return new ResultResponse
            {
                IsSuccess = true
            };

        }
    }
}
