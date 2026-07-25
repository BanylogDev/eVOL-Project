using eVOL.Application.DTOs.Requests.UserDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.RegisterUser
{
    public record RegisterUserCommand(Register Dto) : IRequest<ResultResponse>;
}
