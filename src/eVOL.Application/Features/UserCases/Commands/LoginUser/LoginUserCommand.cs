using eVOL.Application.DTOs;
using eVOL.Application.DTOs.Responses.User;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.LoginUser
{
    public record LoginUserCommand(LoginDTO Dto) : IRequest<LoginUserResponse?>;
}
