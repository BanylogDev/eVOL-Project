using eVOL.Application.DTOs.Responses.Global;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Commands.AdminUnBanUser
{
    public record AdminUnBanUserCommand(Guid Id) : IRequest<ResultResponse>;
}
