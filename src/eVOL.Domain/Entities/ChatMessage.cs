namespace eVOL.Domain.Entities
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public string Text { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
