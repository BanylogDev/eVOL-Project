using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class TransferOwnershipOfCGDTO
    {
        [Required]
        public int ChatGroupId { get; set; }
        [Required]
        public int CurrentOwnerId { get; set; }
        [Required]
        public int NewOwnerId { get; set; }
    }
}
