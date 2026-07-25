namespace eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer
{
    public class UserLoginFields
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public byte[]? RowVersion { get; init; }
    }
}
