using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.FeaturesCases.Admin.Commands.AdminDeleteUser
{
    public class AdminDeleteUserHandler : IRequestHandler<AdminDeleteUserCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<AdminDeleteUserHandler> _logger;

        public AdminDeleteUserHandler(IPostgreUnitOfWork uow, ILogger<AdminDeleteUserHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(AdminDeleteUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Admin -> Started Deletion of user {UserId}", request.Id);

            if (!await _uow.Admin.DeleteUser(request.Id, ct))
            {
                _logger.LogWarning("Admin -> Failed to delete user {UserId}", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = $"Admin -> User not found with id {request.Id}"
                };
            }

            _logger.LogInformation("Admin -> Success, Ended Deletion of user {UserId}", request.Id);

            return new ResultResponse
            {
                IsSuccess = true,
            };

        }
    }
}
