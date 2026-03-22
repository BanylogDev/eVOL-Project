using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Commands.CreateSupportTicket
{
    public class CreateSupportTicketValidator : AbstractValidator<CreateSupportTicketCommand>
    {
        public CreateSupportTicketValidator()
        {
            RuleFor(s => s.Dto.Category)
                .NotEmpty()
                .WithMessage("Category is required.");
            RuleFor(s => s.Dto.Text)
                .NotEmpty()
                .WithMessage("Text is required.");
            RuleFor(s => s.Dto.OpenedBy)
                .NotEmpty()
                .WithMessage("User ID is required to create a support ticket.");
        }
    }
}
