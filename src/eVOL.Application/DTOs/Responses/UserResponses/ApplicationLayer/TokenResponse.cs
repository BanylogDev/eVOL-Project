namespace eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer
{
    public sealed class TokenResponse
    {
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
    }
}
