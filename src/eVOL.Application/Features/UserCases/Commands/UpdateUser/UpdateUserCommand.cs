using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUser
{
    public record UpdateUserCommand(UpdateDTO Dto) : IRequest<User?>;
}
