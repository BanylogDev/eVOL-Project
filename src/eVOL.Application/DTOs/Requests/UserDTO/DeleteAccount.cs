namespace eVOL.Application.DTOs.Requests.UserDTO
{
    public sealed class DeleteAccount
    {
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
    }
}
