using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup
{
    public class TransferOwnershipOfChatGroupHandler : IRequestHandler<TransferOwnershipOfChatGroupCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<TransferOwnershipOfChatGroupHandler> _logger;

        public TransferOwnershipOfChatGroupHandler(IPostgreUnitOfWork uow, ILogger<TransferOwnershipOfChatGroupHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(TransferOwnershipOfChatGroupCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Started transfering chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}", request.Dto.ChatGroupId, request.CurrentOwnerId, request.Dto.NewOwnerId);

            if (!await _uow.Users.CheckUserExistance(request.Dto.NewOwnerId, ct))
            {
                _logger.LogWarning("New owner with id: {UserId} not found.", request.Dto.NewOwnerId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "New Owner not found"
                };
            }


            if (!await _uow.ChatGroup.TransferChatGroupOwnership(request.CurrentOwnerId, request.Dto.ChatGroupId, request.Dto.NewOwnerId, ct))
            {
                _logger.LogError("Error, Something went wrong while transfering the chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}", request.Dto.ChatGroupId, request.CurrentOwnerId, request.Dto.NewOwnerId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Something went wrong."
                };
            }

            _logger.LogInformation("Ended transfering chat group ownership with id: {ChatGroupId} from user with id: {CurrentOwnerUserId} to user with id: {NewOwnerUserId}, Success!", request.Dto.ChatGroupId, request.CurrentOwnerId, request.Dto.NewOwnerId);

            return new ResultResponse
            {
                IsSuccess = true
            };
        }
    }
}
