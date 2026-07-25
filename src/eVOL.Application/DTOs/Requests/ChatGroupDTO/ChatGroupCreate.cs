namespace eVOL.Application.DTOs.Requests.ChatGroupDTO
{
    public sealed class ChatGroupCreate
    {
        public string Name { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
    }
}
