using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup
{
    public class RemoveUserFromChatGroupHandler : IRequestHandler<RemoveUserFromChatGroupCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<RemoveUserFromChatGroupHandler> _logger;

        public RemoveUserFromChatGroupHandler(IPostgreUnitOfWork uow, ILogger<RemoveUserFromChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(RemoveUserFromChatGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started removing user with id: {UserId} from chat group with name {ChatGroupName}", request.UserId, request.ChatGroupName);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.UserId);

                var chatGroup = await _uow.ChatGroup.GetChatGroupByName(request.ChatGroupName);

                if (user == null || chatGroup == null || !chatGroup.GroupUsers.Contains(user) || chatGroup.OwnerId == user.UserId)
                {

                    _logger.LogWarning("Chat group with name: {ChatGroupName} or user with id: {UserId} wasn't found, or user isn't in the group or user is the owner", request.ChatGroupName, request.UserId);

                    return null;
                }

                _logger.LogInformation("Removing user with id: {UserId} from chat group with name {ChatGroupName}, Previous Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

                chatGroup.GroupUsers.Remove(user);
                chatGroup.TotalUsers -= 1;

                _logger.LogInformation("Finished removing user with id: {UserId} from chat group with name {ChatGroupName}, New Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

                await _uow.CommitAsync();

                _logger.LogInformation("Ended removing user with id: {UserId} from chat group with name {ChatGroupName}, Success!", request.UserId, request.ChatGroupName);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogInformation(ex, "Error, Something went wrong during the subtraction of user with id: {UserId} from chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);
                throw;
            }
        }
    }
}
