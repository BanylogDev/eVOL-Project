using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup
{
    public class CreateChatGroupHandler : IRequestHandler<CreateChatGroupCommand, ChatGroup>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<CreateChatGroupHandler> _logger;

        public CreateChatGroupHandler(IPostgreUnitOfWork uow, ILogger<CreateChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroup> Handle(CreateChatGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started creating chat group with name: {ChatGroupName}", request.Dto.Name);

            await _uow.BeginTransactionAsync();

            try
            {

                var chatGroup = new ChatGroup
                {
                    Id = Guid.NewGuid(),
                    Name = request.Dto.Name,
                    TotalUsers = request.Dto.TotalUsers,
                    GroupUsers = request.Dto.GroupUsers,
                    OwnerId = request.Dto.OwnerId,
                    CreatedAt = DateTime.UtcNow,
                };

                _logger.LogInformation("Creating chat group with name: {ChatGroupName}", request.Dto.Name);

                await _uow.ChatGroup.CreateChatGroup(chatGroup);
                await _uow.CommitAsync();

                _logger.LogInformation("Finished creating chat group with name: {ChatGroupName}, Success!", request.Dto.Name);

                return chatGroup;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "Error, Something went wrong during the creation of the chat group with name: {ChatGroupName}", request.Dto.Name);
                throw;
            }
        }
    }
}
