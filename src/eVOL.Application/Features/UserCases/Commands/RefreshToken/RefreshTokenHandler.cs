using eVOL.Application.DTOs.Requests;
using eVOL.Application.Options;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.RepositoriesInteraces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eVOL.Application.Features.UserCases.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenDTO?>
    {

        private readonly IJwtService _jwtService;
        private readonly IPostgreUnitOfWork _uow;
        private readonly IOptions<JwtOptions> _options;
        private readonly ILogger<RefreshTokenHandler> _logger;

        public RefreshTokenHandler(IJwtService jwtService, IPostgreUnitOfWork uow, IOptions<JwtOptions> options, ILogger<RefreshTokenHandler> logger)
        {
            _jwtService = jwtService;
            _uow = uow;
            _options = options;
            _logger = logger;
        }

        public async Task<TokenDTO?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RefreshTokenUseCase");

            await _uow.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Validating expired access token");
                var principal = _jwtService.GetPrincipalFromExpiredToken(request.Dto.AccessToken, _options);
                if (principal == null)
                {
                    _logger.LogWarning("RefreshTokenUseCase failed: Invalid access token");
                    return null;
                }

                _logger.LogInformation("Retrieving user information from token");

                var name = principal.Identity?.Name;
                var user = await _uow.Users.GetUserByName(name);

                if (user == null || user.RefreshToken != request.Dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("RefreshTokenUseCase failed: Invalid refresh token or user not found");
                    return null;
                }

                _logger.LogInformation("Generating new tokens for User ID: {UserId}", user.UserId);

                var newAccessToken = _jwtService.GenerateJwtToken(user, _options);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                _logger.LogInformation("Updating tokens for User ID: {UserId}", user.UserId);

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

                _logger.LogInformation("Committing transaction for RefreshTokenUseCase");

                await _uow.CommitAsync();

                _logger.LogInformation("RefreshTokenUseCase completed successfully for User ID: {UserId}", user.UserId);

                return new TokenDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                };
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, "RefreshTokenUseCase failed and rolled back");
                throw;
            }
        }
    }
}
