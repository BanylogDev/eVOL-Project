namespace eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer
{
    public class BaseUserResponse
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }

        public BaseUserResponse() { }
    }
}
