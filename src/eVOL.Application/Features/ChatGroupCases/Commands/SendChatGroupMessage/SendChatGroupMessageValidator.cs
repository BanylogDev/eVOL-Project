using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.SendChatGroupMessage
{
    public class SendChatGroupMessageValidator : AbstractValidator<SendChatGroupMessageCommand>
    {
        public SendChatGroupMessageValidator()
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .WithMessage("Message is required.")
                .MaximumLength(1000)
                .WithMessage("Message cannot exceed 1000 characters.");

            RuleFor(c => c.ChatGroupName)
                .NotEmpty()
                .WithMessage("ChatGroupName is required.")
                .MaximumLength(100)
                .WithMessage("ChatGroupName cannot exceed 100 characters.");
            
            RuleFor(u => u.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

        }
    }
}
