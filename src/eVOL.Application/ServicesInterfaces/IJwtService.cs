using eVOL.Application.DTOs.ServicesDTOs;
using System.Security.Claims;

namespace eVOL.Application.ServicesInterfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(JwtGeneration user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
