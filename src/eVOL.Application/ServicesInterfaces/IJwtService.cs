using eVOL.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace eVOL.Application.ServicesInterfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user, IConfiguration config);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, IConfiguration config);
    }
}
