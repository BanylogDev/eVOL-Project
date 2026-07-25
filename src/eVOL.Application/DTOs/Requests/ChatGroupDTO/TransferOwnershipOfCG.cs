namespace eVOL.Application.DTOs.Requests.ChatGroupDTO
{
    public sealed class TransferOwnershipOfCG
    {
        public Guid ChatGroupId { get; set; }
        public Guid NewOwnerId { get; set; }
    }
}
