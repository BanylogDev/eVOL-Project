using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.LeaveChatGroup
{
    public class LeaveChatGroupHandler : IRequestHandler<LeaveChatGroupCommand, ChatGroupResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<LeaveChatGroupHandler> _logger;

        public LeaveChatGroupHandler(IPostgreUnitOfWork uow, ILogger<LeaveChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroupResponse> Handle(LeaveChatGroupCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started removing user with id: {UserId} from chat group with name {ChatGroupName}", request.UserId, request.ChatGroupName);


            var user = await _uow.Users.GetUserForChatGroup(request.UserId, ct);

            var chatGroup = await _uow.ChatGroup.GetChatGroupUsersByName(request.ChatGroupName, ct);

            if (chatGroup == null)
            {
                _logger.LogWarning("Chat group with name: {ChatGroupName} not found", request.ChatGroupName);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "Chat Group not found."
                };
            }

            if (user == null)
            {
                _logger.LogWarning("User with id: {UserId} wasn't found", request.UserId);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            if (!chatGroup.Users.Contains(user))
            {
                _logger.LogWarning("User with id: {UserId} isn't in the group!", request.UserId);
                return new ChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "User isnt in the group."
                };
            }

            if (chatGroup.TotalUsers - 1 == 0)
            {
                await _uow.ChatGroup.DeleteChatGroupByName(request.ChatGroupName, request.UserId, ct);
                _logger.LogInformation("Chat Group with name: {ChatGroupName} has been deleted because all users in it left!", request.ChatGroupName);
                return new ChatGroupResponse
                {
                    Name = user.Name,
                    IsSuccess = true,
                };
            }

            _logger.LogInformation("Removing user with id: {UserId} from chat group with name {ChatGroupName}, Previous Total Users: {TotalUsers}", request.UserId, request.ChatGroupName, chatGroup.TotalUsers);

            await _uow.BeginTransactionAsync();

            try
            {   

                chatGroup.Users.Remove(user);
                chatGroup.TotalUsers -= 1;

                if (request.UserId == chatGroup.OwnerId)
                {
                    var usersList = chatGroup.Users.ToList();
                    var random = new Random();

                    var newOwner = usersList[random.Next(usersList.Count)];
                    chatGroup.OwnerId = newOwner.UserId;

                    _logger.LogInformation(
                        "Owner left group. New owner assigned: {NewOwnerId}",
                        newOwner.UserId
                    );
                }

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
