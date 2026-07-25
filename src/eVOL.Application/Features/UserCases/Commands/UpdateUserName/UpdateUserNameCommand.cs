using eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUser
{
    public record UpdateUserNameCommand(Guid Id, UpdateName Dto) : IRequest<ResultResponse>;
}
