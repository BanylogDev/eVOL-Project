using System.ComponentModel.DataAnnotations;

namespace eVOL.Application.DTOs.Requests
{
    public sealed class RegisterDTO
    {
        [Required]
        [StringLength(12, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string AddressName { get; set; }
        [Required]
        public string AddressNumber { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
