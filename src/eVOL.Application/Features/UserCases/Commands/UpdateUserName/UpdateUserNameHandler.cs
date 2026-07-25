using eVOL.Application.DTOs.Responses.Global;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUser
{
    public class UpdateUserNameHandler : IRequestHandler<UpdateUserNameCommand, ResultResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<UpdateUserNameHandler> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserNameHandler(IPostgreUnitOfWork uow, ILogger<UpdateUserNameHandler> logger, IPasswordHasher passwordHasher)

        {
            _uow = uow;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultResponse> Handle(UpdateUserNameCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting UpdateUserNameUseCase for User ID: {UserId}", request.Id);

            var user = await _uow.Auth.GetUserPasswordById(request.Id, ct);

            if (user == null)
            {
                _logger.LogWarning("UpdateUserNameUseCase Error: User ID {UserId} not found", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "User not found"
                };
            }


            if (!_passwordHasher.VerifyPassword(request.Dto.CurrentPassword, user.Password))
            {
                _logger.LogWarning(
                    "Failed name update for user {UserId}: incorrect password.",
                    request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "Password is incorrect!"
                };
            }

            if (!await _uow.Users.UpdateUserName(request.Id, request.Dto.NewName, user.RowVersion, ct))
            {
                _logger.LogError("UpdateUserNameUseCase Error for User ID: {UserId}", request.Id);
                return new ResultResponse
                {
                    IsSuccess = false,
                    Error = "An error occurred while processing your request. Please try again later."
                };
            }

            _logger.LogInformation("UpdateUserNameUseCase completed successfully for User ID: {UserId}", request.Id);

            return new ResultResponse
            {
                IsSuccess = true
            };

        }
    }
}
