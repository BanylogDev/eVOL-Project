using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUserEmail
{
    public class UpdateUserEmailHandler : IRequestHandler<UpdateUserEmailCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UpdateUserEmailHandler> _logger;

        public UpdateUserEmailHandler(IPostgreUnitOfWork uow, ILogger<UpdateUserEmailHandler> logger, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultResponse> Handle(UpdateUserEmailCommand request, CancellationToken ct)
        {

            _logger.LogInformation("Starting UpdateUserEmailUseCase for User ID: {UserId}", request.Id);

            var existingEmail = await _uow.Users.GetUserIdByEmail(request.Dto.NewEmail, ct);

            if (existingEmail != null)
            {
                _logger.LogWarning("UpdateUserEmailUseCase Error: Email {Email} already exists for User ID: {UserId}", request.Dto.NewEmail, existingEmail.UserId);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Email already exists"
                };
            }

            var user = await _uow.Auth.GetUserPasswordById(request.Id, ct);

            if (user == null)
            {
                _logger.LogWarning("UpdateUserEmailUseCase Error: User ID {UserId} not found", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found"
                };
            }


            if (!_passwordHasher.VerifyPassword(request.Dto.CurrentPassword, user.Password))
            {
                _logger.LogWarning(
                    "Failed email update for user {UserId}: incorrect password.",
                    request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Password is incorrect!"
                };
            }

            if (!await _uow.Users.UpdateUserEmail(request.Id, request.Dto.NewEmail, user.RowVersion, ct))
            {
                _logger.LogError("UpdateUserEmailUseCase Error for User ID: {UserId}", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "An error occurred while processing your request. Please try again later."
                };
            }

            _logger.LogInformation("UpdateUserEmailUseCase completed successfully for User ID: {UserId}", request.Id);

            return new ResultResponse
            {
                IsSuccess = true
            };
        }
    }
}
