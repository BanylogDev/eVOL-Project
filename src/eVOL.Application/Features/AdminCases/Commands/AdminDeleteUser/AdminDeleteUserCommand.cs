using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser
{
    public record AdminDeleteUserCommand(int Id) : IRequest<User?>;
}
