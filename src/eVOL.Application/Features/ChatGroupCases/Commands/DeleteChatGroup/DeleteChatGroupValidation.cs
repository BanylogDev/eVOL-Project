using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.DeleteChatGroup
{
    public class DeleteChatGroupValidation : AbstractValidator<DeleteChatGroupCommand>
    {
        public DeleteChatGroupValidation()
        {
            RuleFor(c => c.Dto.ChatGroupId)
                .NotEmpty()
                .WithMessage("ChatGroupId is required.");

            RuleFor(c => c.Dto.ChatGroupOwnerId)
                .NotEmpty()
                .WithMessage("ChatGroupOwnerId is required.");
        }
    }
}
