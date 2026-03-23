using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById
{
    public record GetChatGroupByIdQuery(Guid Id) : IRequest<ChatGroup?>;
}
