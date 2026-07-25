using eVOL.Application.DTOs.Requests.UserDTO;
using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.LoginUser
{
    public record LoginUserCommand(Login Dto) : IRequest<LoginUserResponse>;
}
