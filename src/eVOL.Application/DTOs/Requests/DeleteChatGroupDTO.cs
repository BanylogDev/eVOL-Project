using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs
{
    public sealed class DeleteChatGroupDTO
    {
        [Required]
        public Guid ChatGroupId { get; set; }
        [Required]
        public Guid ChatGroupOwnerId { get; set; }
    }
}
