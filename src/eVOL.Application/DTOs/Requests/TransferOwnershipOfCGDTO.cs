using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class TransferOwnershipOfCGDTO
    {
        [Required]
        public Guid ChatGroupId { get; set; }
        [Required]
        public Guid CurrentOwnerId { get; set; }
        [Required]
        public Guid NewOwnerId { get; set; }
    }
}
