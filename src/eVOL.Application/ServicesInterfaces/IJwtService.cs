using eVOL.Application.Options;
using eVOL.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace eVOL.Application.ServicesInterfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user, IOptions<JwtOptions> options);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, IOptions<JwtOptions> options);
    }
}
