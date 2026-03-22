using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class ClaimSupportTicketDTO
    {
        [Required] 
        public int Id { get; set; }
        [Required]
        public int OpenedBy { get; set; }
    }
}
