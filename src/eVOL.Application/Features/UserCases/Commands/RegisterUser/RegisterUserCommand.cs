using eVOL.Application.DTOs.Requests;
using eVOL.Application.DTOs.Responses.User;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterDTO Dto) : IRequest<RegisterUserResponse?>;
}
