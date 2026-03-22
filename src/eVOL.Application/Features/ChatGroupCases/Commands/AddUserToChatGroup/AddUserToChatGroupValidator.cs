using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.AddUserToChatGroup
{
    public class AddUserToChatGroupValidator : AbstractValidator<AddUserToChatGroupCommand>
    {
        public AddUserToChatGroupValidator()
        {
            RuleFor(c => c.ChatGroupName)
                .NotEmpty()
                .WithMessage("ChatGroupName is required.")
                .MaximumLength(100)
                .WithMessage("ChatGroupName cannot exceed 100 characters.");
        }
    }
    
}
