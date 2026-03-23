using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class SupportTicketDTO
    {
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public Guid OpenedBy { get; set; }
    }
}
