using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.TransferOwnershipOfChatGroup
{
    public class TransferOwnershipOfChatGroupValidation : AbstractValidator<TransferOwnershipOfChatGroupCommand>
    {
        public TransferOwnershipOfChatGroupValidation()
        {
            RuleFor(c => c.Dto.ChatGroupId)
                .NotEmpty()
                .WithMessage("ChatGroupId is required.");

            RuleFor(u => u.Dto.CurrentOwnerId)
                .NotEmpty()
                .WithMessage("CurrentOwnerId is required.");

            RuleFor(u => u.Dto.NewOwnerId)
                .NotEmpty()
                .WithMessage("NewOwnerId is required.");    
        }
    }
}
