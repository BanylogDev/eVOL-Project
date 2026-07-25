namespace eVOL.Application.DTOs.Requests.UserDTO.UpdateDTO
{
    public sealed class UpdateName
    {
        public string? NewName { get; init; }
        public string? CurrentPassword { get; set; }
    }
}
