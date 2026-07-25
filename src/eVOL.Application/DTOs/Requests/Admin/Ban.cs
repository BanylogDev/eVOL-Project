namespace eVOL.Application.DTOs.Requests.Admin
{
    public class Ban
    {
        public Guid UserId { get; set; }
        public DateTime BannedUntil { get; set; }
        public string? Reason { get; set; }
    }
}
