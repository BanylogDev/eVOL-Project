using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.AdminCases.Commands.AdminBanUser
{
    public class AdminBanUserHandler : IRequestHandler<AdminBanUserCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AdminBanUserHandler> _logger;

        public AdminBanUserHandler(IPostgreUnitOfWork uow, ILogger<AdminBanUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(AdminBanUserCommand request, CancellationToken ct)
        {

            _logger.LogInformation("AdminBanUserHandler.Handle: Start handling AdminBanUserCommand for adminId: {adminId}", request.AdminId);

            if (!await _uow.Admin.BanUser(request.Dto.UserId, request.AdminId, request.Dto.BannedUntil, request.Dto.Reason, ct))
            {
                _logger.LogWarning("AdminBanUserHandler.Handle: Failed to ban user with userId: {userId} by adminId: {adminId}", request.Dto.UserId, request.AdminId);
                return new ResultResponse { IsSuccess = false, Error = "Failed to ban user." };
            }

            _logger.LogInformation("AdminBanUserHandler.Handle: Successfully banned user with userId: {userId} by adminId: {adminId}", request.Dto.UserId, request.AdminId);

            return new ResultResponse { IsSuccess = true };


        }
    }
}
