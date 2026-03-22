using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class TokenDTO
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
