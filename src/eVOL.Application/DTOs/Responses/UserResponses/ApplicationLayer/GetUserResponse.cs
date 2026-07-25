using eVOL.Domain.ValueObjects;

namespace eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer
{
    public sealed class GetUserResponse : BaseUserResponse
    {
        public Address? Address { get; set; }
        public Money? Money { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
