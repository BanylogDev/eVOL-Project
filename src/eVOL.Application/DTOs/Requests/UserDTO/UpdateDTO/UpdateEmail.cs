namespace eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO
{
    public sealed class UpdateEmail
    {
        public string? NewEmail { get; init; }
        public string? CurrentPassword { get; set; }
    }
}
