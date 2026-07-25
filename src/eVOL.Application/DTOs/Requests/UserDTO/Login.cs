namespace eVOL.Application.DTOs.Requests.UserDTO
{
    public sealed class Login
    {
        public string? Email { get; init; }
        public string? Password { get; init; }
    }
}
