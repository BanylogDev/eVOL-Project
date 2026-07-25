using eVOL.Application.DTOs.Requests.Admin;
using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Commands.AdminBanUser
{
    public record AdminBanUserCommand(Ban Dto, Guid AdminId) : IRequest<ResultResponse>;
}
