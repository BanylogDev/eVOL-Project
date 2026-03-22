using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Commands.SendSupportTicketMessage
{
    public class SendSupportTicketMessageValidation : AbstractValidator<SendSupportTicketMessageCommand>
    {
        public SendSupportTicketMessageValidation()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("Message text cannot be empty.")
                .MaximumLength(1000)
                .WithMessage("Message text cannot exceed 1000 characters.");

            RuleFor(x => x.SupportTicketName)
                .NotEmpty()
                .WithMessage("Support ticket name cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Support ticket name cannot exceed 100 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required to send a message to the support ticket.");
        }
    }
}
