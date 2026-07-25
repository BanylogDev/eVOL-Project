using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.UserCases.Queries.GetUser
{
    public record GetUserQuery(Guid Id) : IRequest<GetUserResponse>;
}
