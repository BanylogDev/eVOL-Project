using eVOL.Application.DTOs.Responses.User;
using MediatR;

namespace eVOL.Application.Features.UserCases.Queries.GetUser
{
    public record GetUserQuery(int Id) : IRequest<GetUserResponse?>;
}
