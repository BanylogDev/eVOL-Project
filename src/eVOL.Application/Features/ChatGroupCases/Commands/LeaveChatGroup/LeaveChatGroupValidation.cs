using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.LeaveChatGroup
{
    public class LeaveChatGroupValidation : AbstractValidator<LeaveChatGroupCommand>
    {
        public LeaveChatGroupValidation()
        {
            RuleFor(u => u.UserId)
                .NotEmpty()
                .WithMessage("User Id is required.");
            RuleFor(c => c.ChatGroupName)
                .NotEmpty()
                .WithMessage("Chat Group Name is required.")
                .MaximumLength(100)
                .WithMessage("ChatGroupName cannot exceed 100 characters.");
        }
    }
}
