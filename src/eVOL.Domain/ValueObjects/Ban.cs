namespace eVOL.Domain.ValueObjects
{
    public class Ban
    {
        public bool IsBanned { get; private set; }
        public DateTime BannedUntil { get; private set; }
        public Guid BannedBy { get; private set; }
        public string? Reason { get; private set; }

        public Ban(bool isBanned, DateTime bannedUntil, Guid bannedBy, string? reason)
        {
            IsBanned = isBanned;
            BannedUntil = bannedUntil;
            BannedBy = bannedBy;
            Reason = reason;
        }
    }
}
