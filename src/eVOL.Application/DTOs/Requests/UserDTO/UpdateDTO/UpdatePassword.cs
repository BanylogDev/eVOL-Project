namespace eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO
{
    public sealed class UpdatePassword
    {
        public string? CurrentPassword { get; init; }
        public string? NewPassword { get; init; }
        public string? ConfirmNewPassword { get; init; }
    }
}
