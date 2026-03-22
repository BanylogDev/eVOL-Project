using eVOL.Application.DTOs.Requests;
using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.UserCases.Commands.DeleteUser
{
    public record DeleteUserCommand(DeleteAccountDTO Dto) : IRequest<User?>;
}
