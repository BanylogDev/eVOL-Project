
using eVOL.Application.DTOs.Responses.ChatGroupResponses.InfrastructureLayer;
using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Domain.Entities;
using System.Text.Json.Serialization;

namespace eVOL.Infrastructure.Serialization;

[JsonSerializable(typeof(UserFields))]
[JsonSerializable(typeof(GetChatGroup))]
[JsonSerializable(typeof(ChatGroupUsers))]
[JsonSerializable(typeof(ChatMessage))]
public partial class CacheJsonContext : JsonSerializerContext
{
}