using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public class AddUserToChatGroupValidator : AbstractValidator<AddUserToChatGroupCommand>
    {
        public AddUserToChatGroupValidator()
        {

            RuleFor(u => u.UserId)
                .NotEmpty()
                .WithMessage("User ID is required to delete a user.");

            RuleFor(c => c.ChatGroupName)
                .NotEmpty()
                .WithMessage("ChatGroupName is required.")
                .MaximumLength(100)
                .WithMessage("ChatGroupName cannot exceed 100 characters.");
        }
    }

}
