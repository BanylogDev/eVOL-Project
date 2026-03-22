using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup
{
    public class TransferOwnershipOfChatGroupHandler : IRequestHandler<TransferOwnershipOfChatGroupCommand, ChatGroup?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<TransferOwnershipOfChatGroupHandler> _logger;

        public TransferOwnershipOfChatGroupHandler(IPostgreUnitOfWork uow, ILogger<TransferOwnershipOfChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroup?> Handle(TransferOwnershipOfChatGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started transfering chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}", request.Dto.ChatGroupId, request.Dto.CurrentOwnerId, request.Dto.NewOwnerId);

            await _uow.BeginTransactionAsync();

            try
            {
                var currentOwner = await _uow.Users.GetUserById(request.Dto.CurrentOwnerId);

                var newOwner = await _uow.Users.GetUserById(request.Dto.NewOwnerId);

                var chatGroup = await _uow.ChatGroup.GetChatGroupById(request.Dto.ChatGroupId);

                if (currentOwner == null || newOwner == null || chatGroup == null || chatGroup.OwnerId != request.Dto.CurrentOwnerId)
                {
                    _logger.LogWarning("Current chat group owner with id: {CurrentOwnerUserId} or New chat group owner with id: {NewOwnerUserId} or chat group with id: {ChatGroupId} weren't found or user trying to transfer ownership isn't the actual owner", request.Dto.CurrentOwnerId, request.Dto.NewOwnerId, request.Dto.ChatGroupId);
                    return null;
                }

                _logger.LogInformation("Transfering chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}", request.Dto.ChatGroupId, request.Dto.CurrentOwnerId, request.Dto.NewOwnerId);

                chatGroup.OwnerId = request.Dto.NewOwnerId;

                _logger.LogInformation("Finished transfering chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}", request.Dto.ChatGroupId, request.Dto.CurrentOwnerId, request.Dto.NewOwnerId);

                await _uow.CommitAsync();

                _logger.LogInformation("Ended transfering chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}, Success!", request.Dto.ChatGroupId, request.Dto.CurrentOwnerId, request.Dto.NewOwnerId);

                return chatGroup;
            }
            catch (Exception ex) 
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "Error, Something went wrong while transfering the chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}", request.Dto.ChatGroupId, request.Dto.CurrentOwnerId, request.Dto.NewOwnerId);
                throw;
            }
        }
    }
}
