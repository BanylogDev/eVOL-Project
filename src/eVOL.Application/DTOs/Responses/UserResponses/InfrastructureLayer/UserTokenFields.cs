namespace eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer
{
    public class UserTokenFields
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public byte[]? RowVersion { get; init; }
    }
}
