using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(IPostgreUnitOfWork uow, IPasswordHasher passwordHasher, ILogger<DeleteUserHandler> logger)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting DeleteUserUseCase for User ID: {UserId}", request.Id);

            var user = await _uow.Auth.GetUserPasswordById(request.Id, ct);

            if (user == null)
            {
                _logger.LogWarning("DeleteUserUseCase failed: User not found.");
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found."
                };
            }

            if (!_passwordHasher.VerifyPassword(request.Dto.Password, user.Password))
            {
                _logger.LogWarning("DeleteUserUseCase failed: Password mismatch.");
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Password mismatch."
                };
            }

            _logger.LogInformation("Deleting User ID: {UserId}", request.Id);

            if (!await _uow.Users.DeleteUser(request.Id, user.RowVersion, ct))
            {
                _logger.LogError("DeleteUserUseCase failed for User ID: {UserId}", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Conflict! RowVersion mismatch"
                };
            }

            _logger.LogInformation("DeleteUserUseCase completed successfully for User ID: {UserId}", request.Id);

            return new ResultResponse
            {
                IsSuccess = true,
            };

        }
    }
}
