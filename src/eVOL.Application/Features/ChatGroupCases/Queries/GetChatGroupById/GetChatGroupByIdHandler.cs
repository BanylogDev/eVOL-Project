using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById
{
    public class GetChatGroupByIdHandler : IRequestHandler<GetChatGroupByIdQuery, ChatGroup?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<GetChatGroupByIdHandler> _logger;

        public GetChatGroupByIdHandler(IPostgreUnitOfWork uow, ILogger<GetChatGroupByIdHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ChatGroup?> Handle(GetChatGroupByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started geting chat group with id: {ChatGroupId}", request.Id);

            var chatGroup = await _uow.ChatGroup.GetChatGroupById(request.Id);

            if (chatGroup == null)
            {
                _logger.LogWarning("Chat group wasn't found with id: {ChatGroupId}", request.Id);
                return null;
            }

            _logger.LogInformation("Ended getting chat group with id: {ChatGroupId}, Success!", request.Id);

            return chatGroup;
        }
    }
}
