using eVOL.Application.DTOs.Requests.UserDTO;
using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.RefreshToken
{
    public record RefreshTokenCommand(Token Dto) : IRequest<TokenResponse>;

}
