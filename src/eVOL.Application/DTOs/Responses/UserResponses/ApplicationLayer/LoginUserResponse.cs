using eVOL.Application.DTOs.Responses.Global;

namespace eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer
{
    public sealed class LoginUserResponse : ResultResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
