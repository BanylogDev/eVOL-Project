using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class ClaimSupportTicketDTO
    {
        [Required] 
        public Guid Id { get; set; }
        [Required]
        public Guid OpenedBy { get; set; }
    }
}
