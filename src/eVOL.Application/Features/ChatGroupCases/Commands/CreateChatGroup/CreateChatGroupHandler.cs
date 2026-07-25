using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup
{
    public class CreateChatGroupHandler : IRequestHandler<CreateChatGroupCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<CreateChatGroupHandler> _logger;

        public CreateChatGroupHandler(IPostgreUnitOfWork uow, ILogger<CreateChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(CreateChatGroupCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started creating chat group with name: {ChatGroupName}", request.Dto.Name);

            var user = await _uow.Users.GetUserForChatGroup(request.Id, ct);

            if (user == null)
            {
                _logger.LogWarning("User with id: {UserId} not found.", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            var usersList = new List<ChatGroupUser>();
            usersList.Add(user);

            var chatGroup = new ChatGroup
            {
                Id = Guid.NewGuid(),
                Name = request.Dto.Name,
                TotalUsers = request.Dto.TotalUsers,
                GroupUsers = usersList,
                OwnerId = request.Id,
                CreatedAt = DateTime.UtcNow,
            };

            if (!await _uow.ChatGroup.CreateChatGroup(chatGroup, ct))
            {
                _logger.LogWarning("Something went wrong during creation process of chatgroup with name: {ChatGroupName}", request.Dto.Name);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Something went wrong."
                };
            }

            _logger.LogInformation("Finished creating chat group with name: {ChatGroupName}, Success!", request.Dto.Name);

            return new ResultResponse
            {
                IsSuccess = true
            };

        }
    }
}
