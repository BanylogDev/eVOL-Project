using eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUserEmail
{
    public record UpdateUserEmailCommand(Guid Id, UpdateEmail Dto) : IRequest<ResultResponse>;
}
