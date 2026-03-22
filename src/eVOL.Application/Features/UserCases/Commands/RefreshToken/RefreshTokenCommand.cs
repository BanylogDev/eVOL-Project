using eVOL.Application.DTOs.Requests;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.RefreshToken
{
    public record RefreshTokenCommand(TokenDTO Dto) : IRequest<TokenDTO?>;

}
