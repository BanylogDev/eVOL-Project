using eVOL.Application.DTOs.Responses.User;
using eVOL.Application.Options;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.RepositoriesInteraces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eVOL.Application.Features.UserCases.Commands.LoginUser
{

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse?>
    {

        private readonly IPostgreUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IOptions<JwtOptions> _options;
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
            _options = options;
            _logger = logger;
        }

        public async Task<LoginUserResponse?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting LoginUserUseCase for Email: {Email}", request.Dto.Email);

            await _uow.BeginTransactionAsync();

            try
            {
                var user = await _uow.Users.GetUserByEmail(request.Dto.Email);

                if (user == null || !_passwordHasher.VerifyPassword(request.Dto.Password, user.Password))
                {
                    _logger.LogWarning("LoginUserUseCase failed: User not found or invalid password for Email: {Email}", request.Dto.Email);
                    return null;
                }

                _logger.LogInformation("Generating tokens for User ID: {UserId}", user.UserId);

                var token = _jwtService.GenerateJwtToken(user, _options);
                var refreshToken = _jwtService.GenerateRefreshToken();

                _logger.LogInformation("Updating tokens for User ID: {UserId}", user.UserId);

                user.RefreshToken = refreshToken;
                user.AccessToken = token;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

                await _uow.CommitAsync();

                _logger.LogInformation("LoginUserUseCase completed successfully for User ID: {UserId}", user.UserId);

                _logger.LogInformation($"handled by {Environment.MachineName}");

                return user.Adapt<LoginUserResponse>();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "LoginUserUseCase failed and rolled back for Email: {Email}", request.Dto.Email);
                throw;
            }
        }
    }
}
