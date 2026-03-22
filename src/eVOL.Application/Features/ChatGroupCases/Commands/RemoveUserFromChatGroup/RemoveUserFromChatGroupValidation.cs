using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.RemoveUserFromChatGroup
{
    public class RemoveUserFromChatGroupValidation : AbstractValidator<RemoveUserFromChatGroupCommand>
    {
        public RemoveUserFromChatGroupValidation()
        {
            RuleFor(u => u.UserId)
                .NotEmpty()
                .WithMessage("User Id is required.");

            RuleFor(c => c.ChatGroupName)
                .NotEmpty()
                .WithMessage("ChatGroupName is required.")
                .MaximumLength(100)
                .WithMessage("ChatGroupName cannot exceed 100 characters.");
        }
    }
}
