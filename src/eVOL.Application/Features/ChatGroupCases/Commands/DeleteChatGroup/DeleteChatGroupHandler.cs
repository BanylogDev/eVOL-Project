using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup
{
    public class DeleteChatGroupHandler : IRequestHandler<DeleteChatGroupCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<DeleteChatGroupHandler> _logger;

        public DeleteChatGroupHandler(IPostgreUnitOfWork uow, ILogger<DeleteChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(DeleteChatGroupCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started deleting chat group with id: {ChatGroupId}", request.Dto.ChatGroupId);

            if (!await _uow.ChatGroup.DeleteChatGroup(request.Dto.ChatGroupId, request.UserId, ct))
            {
                _logger.LogWarning("Error, Something went wrong during the deletion of chat group with id: {ChatGroupId}", request.Dto.ChatGroupId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = ""
                };
            }

            _logger.LogInformation("Ended deleting chat group with id: {ChatGroupId}, Success!", request.Dto.ChatGroupId);

            return new ResultResponse
            {
                IsSuccess = true,
            };
        }
    }
}
