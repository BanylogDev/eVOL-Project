using eVOL.Application.DTOs.Responses.ChatGroupResponses.ApplicationLayer;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById
{
    public record GetChatGroupByIdQuery(Guid Id) : IRequest<GetChatGroupResponse>;
}
