using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketValidation : AbstractValidator<DeleteSupportTicketCommand>
    {
        public DeleteSupportTicketValidation()
        {
            RuleFor(s => s.Id)
                .NotEmpty()
                .WithMessage("SupportTicket ID is required to delete a support ticket.");
        }
    }
}
