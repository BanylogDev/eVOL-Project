namespace eVOL.Application.DTOs.Responses.User
{
    public sealed class LoginUserResponse : BaseUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
