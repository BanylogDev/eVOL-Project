using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public class AddUserToChatGroupHandler : IRequestHandler<AddUserToChatGroupCommand, ChatGroupResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AddUserToChatGroupHandler> _logger;

        public AddUserToChatGroupHandler(IPostgreUnitOfWork uow, ILogger<AddUserToChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroupResponse> Handle(AddUserToChatGroupCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started adding user with id: {UserId} in chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);

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

            if (chatGroup.Users.Contains(user))
            {
                _logger.LogWarning("User with id: {UserId} is already inside the chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "User is already in the group."
                };
            }


            await _uow.BeginTransactionAsync();

            try
            {
                chatGroup.Users.Add(user);
                chatGroup.TotalUsers += 1;

                await _uow.CommitAsync();

                _logger.LogInformation("Ended adding user with id: {UserId} in chat group with name: {ChatGroupName}, Success!", request.UserId, request.ChatGroupName);

                return new ChatGroupResponse
                {
                    IsSuccess = true,
                    Name = user.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while trying adding user with id: {UserId} to chat group with name: {ChatGroupName}", request.UserId, request.ChatGroupName);

                await _uow.RollbackAsync();

                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "Something went wrong."
                };
            }
        }
    }
}
