namespace eVOL.Application.DTOs.Requests.UserDTO
{
    public sealed class Register
    {
        public string? Name { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
        public string? Country { get; init; }
        public string? City { get; init; }
        public string? AddressName { get; init; }
        public string? AddressNumber { get; init; }
        public double? Balance { get; init; }
        public string? Currency { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
