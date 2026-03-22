using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class DeleteAccountDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
