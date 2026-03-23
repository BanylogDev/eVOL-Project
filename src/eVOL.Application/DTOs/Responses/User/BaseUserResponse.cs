namespace eVOL.Application.DTOs.Responses.User
{
    public class BaseUserResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public BaseUserResponse() { }
    }
}
