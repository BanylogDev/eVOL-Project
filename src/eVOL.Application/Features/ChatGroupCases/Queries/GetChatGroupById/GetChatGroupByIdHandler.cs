using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById
{
    public class GetChatGroupByIdHandler : IRequestHandler<GetChatGroupByIdQuery, GetChatGroupResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<GetChatGroupByIdHandler> _logger;

        public GetChatGroupByIdHandler(IPostgreUnitOfWork uow, ILogger<GetChatGroupByIdHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<GetChatGroupResponse> Handle(GetChatGroupByIdQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Started geting chat group with id: {ChatGroupId}", request.Id);

            var chatGroup = await _uow.ChatGroup.GetChatGroupById(request.Id, ct);

            if (chatGroup == null)
            {
                _logger.LogWarning("Chat group wasn't found with id: {ChatGroupId}", request.Id);
                return new GetChatGroupResponse
                {
                    IsSuccess = false,
                    Error = "Chat Group not found."
                };
            }

            _logger.LogInformation("Ended getting chat group with id: {ChatGroupId}, Success!", request.Id);

            return new GetChatGroupResponse
            {
                Id = request.Id,
                Name = chatGroup.Name,
                OwnerId = chatGroup.OwnerId,
                TotalUsers = chatGroup.TotalUsers,
                Users = chatGroup.Users,
                Messages = chatGroup.Messages,
                CreatedAt = chatGroup.CreatedAt,
                IsSuccess = true
            };
        }
    }
}
