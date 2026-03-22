namespace eVOL.Domain.Entities
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<User> GroupUsers { get; set; } = new List<User>();
    }
}
