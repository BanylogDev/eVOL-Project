using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public class AddUserToChatGroupHandler : IRequestHandler<AddUserToChatGroupCommand, User?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AddUserToChatGroupHandler> _logger;

        public AddUserToChatGroupHandler(IPostgreUnitOfWork uow, ILogger<AddUserToChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<User?> Handle(AddUserToChatGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started adding user with id: {UserId} in chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserById(request.UserId);

                var chatGroup = await _uow.ChatGroup.GetChatGroupByName(request.ChatGroupName);

                if (user == null || chatGroup == null || chatGroup.GroupUsers.Contains(user))
                {
                    _logger.LogWarning("User with id: {UserId} or ChatGroup with name: {ChatGroupName} wasn't found or User is already in the group!", request.UserId, request.ChatGroupName);
                    return null;
                }

                _logger.LogInformation("Adding User with id: {UserId} in the chat group with name: {ChatGroupName}, Previous Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

                chatGroup.GroupUsers.Add(user);
                chatGroup.TotalUsers += 1;

                _logger.LogInformation("Finished Adding User with id: {UserId} in the chat group with name: {ChatGroupName}, New Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

                await _uow.CommitAsync();

                _logger.LogInformation("Ended adding user with id: {UserId} in chat group with name: {ChatGroupName}, Success!", request.UserId, request.ChatGroupName);

                return user;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "Error, Something went wrong during the addition of the user with id: {UserId} in the group with name: {ChatGroupName}, Failure!", request.UserId, request.ChatGroupName);
                throw;
            }
        }
    }
}
