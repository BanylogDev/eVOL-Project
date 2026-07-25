using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup
{
    public class RemoveUserFromChatGroupHandler : IRequestHandler<RemoveUserFromChatGroupCommand, ChatGroupResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<RemoveUserFromChatGroupHandler> _logger;

        public RemoveUserFromChatGroupHandler(IPostgreUnitOfWork uow, ILogger<RemoveUserFromChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroupResponse> Handle(RemoveUserFromChatGroupCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started removing user with id: {UserId} from chat group with name {ChatGroupName}", request.UserId, request.ChatGroupName);

            var chatGroup = await _uow.ChatGroup.GetChatGroupUsersByName(request.ChatGroupName, ct);

            if (chatGroup == null)
            {
                _logger.LogWarning("ChatGroup with name: {ChatGroupName} was not found.", request.ChatGroupName);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "ChatGroup not found."
                };
            }

            if (chatGroup.OwnerId != request.OwnerId)
            {
                _logger.LogWarning("User with id: {UserId} is not the owner of the chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "Only the owner can add users."
                };
            }

            var user = await _uow.Users.GetUserForChatGroup(request.UserId, ct);

            if (user == null)
            {
                _logger.LogWarning("User with id: {UserId} was not found.", request.UserId);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            if (!chatGroup.Users.Contains(user))
            {
                _logger.LogWarning("User with id: {UserId} is not inside the chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "User is not in the group."
                };
            }

            _logger.LogInformation("Removing user with id: {UserId} from chat group with name {ChatGroupName}, Previous Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);


            await _uow.BeginTransactionAsync();

            try
            {

                chatGroup.Users.Remove(user);
                chatGroup.TotalUsers -= 1;

                await _uow.CommitAsync();

                _logger.LogInformation("Ended removing user with id: {UserId} from chat group with name {ChatGroupName}, Success!", request.UserId, request.ChatGroupName);

                return new ChatGroupResponse
                {
                    Name = user.Name,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogInformation(ex, "Error, Something went wrong during the subtraction of user with id: {UserId} from chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "Something went wrong."
                };
            }
        }
    }
}
