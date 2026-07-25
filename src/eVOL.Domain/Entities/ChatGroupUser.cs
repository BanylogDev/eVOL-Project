namespace eVOL.Domain.Entities
{
    public class ChatGroupUser
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; }
    }
}
