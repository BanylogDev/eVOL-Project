using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUserPassword
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UpdateUserPasswordHandler> _logger;

        public UpdateUserPasswordHandler(IPostgreUnitOfWork uow, IPasswordHasher passwordHasher, ILogger<UpdateUserPasswordHandler> logger)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<ResultResponse> Handle(UpdateUserPasswordCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting UpdateUserPasswordUseCase for User ID: {UserId}", request.Id);

            var user = await _uow.Auth.GetUserPasswordById(request.Id, ct);

            if (user == null)
            {
                _logger.LogWarning("UpdateUserPasswordUseCase Error: User ID {UserId} not found", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found"
                };
            }

            if (!_passwordHasher.VerifyPassword(request.Dto.CurrentPassword, user.Password))
            {
                _logger.LogWarning(
                    "Failed password update for user {UserId}: incorrect password.",
                    request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Password is incorrect!"
                };
            }


            if (!await _uow.Auth.UpdateUserPassword(request.Id, request.Dto.NewPassword, user.RowVersion, ct))
            {
                _logger.LogError("UpdateUserPasswordUseCase Error for User ID: {UserId}", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "An error occurred while processing your request. Please try again later."
                };
            }

            _logger.LogInformation("UpdateUserPasswordUseCase completed successfully for User ID: {UserId}", request.Id);

            return new ResultResponse
            {
                IsSuccess = true
            };
        }
    }
}
