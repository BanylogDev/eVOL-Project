using eVOL.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class ChatGroupDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int TotalUsers { get; set; }
        [Required]
        public List<User>? GroupUsers { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
    }
}
