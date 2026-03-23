using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser
{
    public record AdminDeleteUserCommand(Guid Id) : IRequest<User?>;
}
