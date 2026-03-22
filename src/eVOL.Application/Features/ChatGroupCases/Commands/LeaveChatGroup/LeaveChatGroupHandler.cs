using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.LeaveChatGroup
{
    public class LeaveChatGroupHandler : IRequestHandler<LeaveChatGroupCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<LeaveChatGroupHandler> _logger;

        public LeaveChatGroupHandler(IPostgreUnitOfWork uow, ILogger<LeaveChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(LeaveChatGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started removing user with id: {UserId} from chat group with name {ChatGroupName}", request.UserId, request.ChatGroupName);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.UserId);

                var chatGroup = await _uow.ChatGroup.GetChatGroupByName(request.ChatGroupName);

                if (chatGroup == null || user == null || !chatGroup.GroupUsers.Contains(user))
                {
                    _logger.LogWarning("Chat group with name: {ChatGroupName} or user with id: {UserId} wasn't found, or user isn't in the group!", request.ChatGroupName, request.UserId);
                    return null;
                }

                _logger.LogInformation("Removing user with id: {UserId} from chat group with name {ChatGroupName}, Previous Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

                chatGroup.GroupUsers.Remove(user);
                chatGroup.TotalUsers -= 1;

                _logger.LogInformation("Finished removing user with id: {UserId} from chat group with name {ChatGroupName}, New Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

                if (chatGroup.TotalUsers == 0)
                {
                    _uow.ChatGroup.DeleteChatGroup(chatGroup);
                    _logger.LogInformation("Chat Group with name: {ChatGroupName} has been deleted because all users in it left!", request.ChatGroupName);
                }

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
