using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup
{
    public class DeleteChatGroupHandler : IRequestHandler<DeleteChatGroupCommand, ChatGroup?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<DeleteChatGroupHandler> _logger;

        public DeleteChatGroupHandler(IPostgreUnitOfWork uow, ILogger<DeleteChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroup?> Handle(DeleteChatGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started deleting chat group with id: {ChatGroupId}", request.Dto.ChatGroupId);

            await _uow.BeginTransactionAsync();

            try
            {
                var chatGroup = await _uow.ChatGroup.GetChatGroupById(request.Dto.ChatGroupId);

                if (chatGroup == null || chatGroup.OwnerId != request.Dto.ChatGroupOwnerId)
                {
                    _logger.LogWarning("Chat Group with id: {ChatGroupId} wasn't found or user that triggered the action with id: {UserId} isn't the owner of the chat group", request.Dto.ChatGroupId, request.Dto.ChatGroupOwnerId);
                    return null;
                }

                _logger.LogInformation("Deleting chat group with name: {ChatGroupName}", chatGroup.Name);

                _uow.ChatGroup.DeleteChatGroup(chatGroup);
                await _uow.CommitAsync();

                _logger.LogInformation("Ended deleting chat group with name: {ChatGroupName}, Success!", chatGroup.Name);

                return chatGroup;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "Error, Something went wrong during the deletion of chat group with id: {ChatGroupId}", request.Dto.ChatGroupId);
                throw;
            }
        }
    }
}
