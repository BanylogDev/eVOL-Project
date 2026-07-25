using eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUserPassword
{
    public record UpdateUserPasswordCommand(Guid Id, UpdatePassword Dto) : IRequest<ResultResponse>;

}
