using eVOL.Application.DTOs.Responses.Admin;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetUser
{
    public record AdminGetUserQuery(Guid Id) : IRequest<GetUserAdminResponse>;

}
