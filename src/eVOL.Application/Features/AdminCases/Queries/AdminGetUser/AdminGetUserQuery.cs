using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetUser
{
    public record AdminGetUserQuery(Guid Id) : IRequest<User?>;

}
