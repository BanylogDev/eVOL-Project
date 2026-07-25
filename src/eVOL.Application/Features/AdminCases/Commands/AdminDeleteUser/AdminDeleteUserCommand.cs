using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser
{
    public record AdminDeleteUserCommand(Guid Id) : IRequest<ResultResponse>;
}
