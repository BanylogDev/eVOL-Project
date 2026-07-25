using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket
{
    public class UnClaimSupportTicketValidation : AbstractValidator<UnClaimSupportTicketCommand>
    {
        public UnClaimSupportTicketValidation()
        {
            RuleFor(s => s.Dto.SupportTicketId)
                .NotEmpty()
                .WithMessage("SupportTicket ID is required.");
        }
    }
}
