using eVOL.Domain.Entities;
using MediatR;

namespace eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById
{
    public record GetChatGroupByIdQuery(int Id) : IRequest<ChatGroup?>;
}
