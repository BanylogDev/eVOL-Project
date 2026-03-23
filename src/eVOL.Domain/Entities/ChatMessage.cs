namespace eVOL.Domain.Entities
{
    public class ChatMessage
    {
        public Guid MessageId { get; set; }
        public string Text { get; set; } = string.Empty;
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
