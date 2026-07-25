using eVOL.Application.DTOs.Requests.UserDTO;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid Id, DeleteAccount Dto) : IRequest<ResultResponse>;
}
