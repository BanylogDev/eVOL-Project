using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs
{
    public sealed class DeleteChatGroupDTO
    {
        [Required]
        public int ChatGroupId { get; set; }
        [Required]
        public int ChatGroupOwnerId { get; set; }
    }
}
