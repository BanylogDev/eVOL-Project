namespace eVOL.Application.DTOs.Requests.UserDTO
{
    public sealed class Token
    {
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
    }
}
