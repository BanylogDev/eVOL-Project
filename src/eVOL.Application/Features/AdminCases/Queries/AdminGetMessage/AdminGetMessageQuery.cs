using eVOL.Application.DTOs.Responses.Message;
using MediatR;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetMessage
{
    public record AdminGetMessageQuery(Guid id) : IRequest<ChatMessageResponse>;
}
