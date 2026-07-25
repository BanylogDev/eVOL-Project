using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using eVOL.Application.DTOs.ServicesDTOs;
using eVOL.Application.Options;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eVOL.Application.Features.UserCases.Commands.LoginUser
{

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginUserHandler> _logger;


        public LoginUserHandler(IPostgreUnitOfWork uow,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IOptions<JwtOptions> options,
            ILogger<LoginUserHandler> logger)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting LoginUserUseCase for Email: {Email}", request.Dto.Email);


            var user = await _uow.Users.GetUserLoginFields(request.Dto.Email, ct);

            if (user == null)
            {
                _logger.LogWarning("LoginUserUseCase failed: User not found for Email: {Email}", request.Dto.Email);
                return new LoginUserResponse
                {
                    IsSuccess = false,
                    Error = "Invalid Email or Password!"
                };
            }

            if (!_passwordHasher.VerifyPassword(request.Dto.Password, user.Password))
            {
                _logger.LogWarning("LoginUserUseCase failed: Invalid password for Email: {Email}", request.Dto.Email);
                return new LoginUserResponse
                {
                    IsSuccess = false,
                    Error = "Invalid Email or Password!"
                };
            }

            _logger.LogInformation("Generating tokens for User ID: {UserId}", user.UserId);

            var accessToken = _jwtService.GenerateJwtToken(new JwtGeneration
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
            var refreshToken = _jwtService.GenerateRefreshToken();

            _logger.LogInformation("Updating tokens for User ID: {UserId}", user.UserId);

            if (!await _uow.Auth.UpdateRefreshToken(user.UserId, refreshToken, DateTime.UtcNow.AddDays(1), user.RowVersion, ct))
            {
                _logger.LogError("LoginUserUseCase failed something went rwong!");
                return new LoginUserResponse
                {
                    IsSuccess = false,
                    Error = "An error occurred while processing your request. Please try again later."
                };
            }

            _logger.LogInformation("LoginUserUseCase completed successfully for User ID: {UserId}", user.UserId);


            return new LoginUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsSuccess = true
            };

        }
    }
}
