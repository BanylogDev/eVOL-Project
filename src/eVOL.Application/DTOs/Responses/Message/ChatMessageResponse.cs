namespace eVOL.Application.DTOs.Responses.Message
{
    public class ChatMessageResponse
    {
        public Guid MessageId { get; set; }
        public string Text { get; set; } = string.Empty;
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
    }
}
