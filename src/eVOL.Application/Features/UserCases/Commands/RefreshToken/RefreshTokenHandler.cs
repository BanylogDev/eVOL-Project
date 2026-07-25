using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using eVOL.Application.DTOs.ServicesDTOs;
using eVOL.Application.Options;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Application.ServicesInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace eVOL.Application.Features.UserCases.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {

        private readonly IJwtService _jwtService;
        private readonly IPostgreUnitOfWork _uow;
        private readonly ILogger<RefreshTokenHandler> _logger;

        public RefreshTokenHandler(IJwtService jwtService, IPostgreUnitOfWork uow, IOptions<JwtOptions> options, ILogger<RefreshTokenHandler> logger)
        {
            _jwtService = jwtService;
            _uow = uow;
            _logger = logger;
        }

        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Starting RefreshTokenUseCase");

            _logger.LogInformation("Validating expired access token");
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.Dto.AccessToken);
            if (principal == null)
            {
                _logger.LogWarning("RefreshTokenUseCase failed: Invalid access token");
                return new TokenResponse
                {
                    IsSuccess = false,
                    Error = "Invalid access token"
                };
            }

            _logger.LogInformation("Retrieving user information from token");

            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                _logger.LogWarning("RefreshTokenUseCase failed: Missing user ID claim");
                return new TokenResponse
                {
                    IsSuccess = false,
                    Error = "Missing user ID claim"
                };
            }

            var userId = Guid.Parse(userIdClaim);

            var user = await _uow.Users.GetUserTokenFields(userId, ct);

            if (user is null)
            {
                _logger.LogWarning("RefreshTokenUseCase failed: User not found");
                return new TokenResponse
                {
                    IsSuccess = false,
                    Error = "User not found"
                };
            }

            if (user.RefreshToken != request.Dto.RefreshToken)
            {
                _logger.LogWarning("RefreshTokenUseCase failed: Refresh token mismatch for User ID: {UserId}", user.UserId);
                return new TokenResponse
                {
                    IsSuccess = false,
                    Error = "Refresh token mismatch"
                };
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("RefreshTokenUseCase failed: Refresh token expired for User ID: {UserId}", user.UserId);
                return new TokenResponse
                {
                    IsSuccess = false,
                    Error = "Refresh token expired"
                };
            }

            _logger.LogInformation("Generating new tokens for User ID: {UserId}", user.UserId);

            var newAccessToken = _jwtService.GenerateJwtToken(new JwtGeneration
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
            });
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            _logger.LogInformation("Updating tokens for User ID: {UserId}", user.UserId);

            if (!await _uow.Auth.UpdateRefreshToken(user.UserId, newRefreshToken, DateTime.UtcNow.AddDays(1), user.RowVersion, ct))
            {
                _logger.LogError("RefreshTokenUseCase failed something went wrong!");
                return new TokenResponse
                {
                    IsSuccess = false,
                    Error = "An error occurred while processing your request. Please try again later."
                };
            }

            _logger.LogInformation("RefreshTokenUseCase completed successfully for User ID: {UserId}", user.UserId);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                IsSuccess = true
            };
        }
    }
}
