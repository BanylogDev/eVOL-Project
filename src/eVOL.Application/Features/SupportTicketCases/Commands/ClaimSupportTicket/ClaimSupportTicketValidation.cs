using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Commands.ClaimSupportTicket
{
    public class ClaimSupportTicketValidation : AbstractValidator<ClaimSupportTicketCommand>
    {
        public ClaimSupportTicketValidation()
        {
            RuleFor(s => s.Dto.Id)
                .NotEmpty()
                .WithMessage("SupportTicket ID is required."); 

            RuleFor(s => s.Dto.OpenedBy)
                .NotEmpty()
                .WithMessage("User ID is required to claim the support ticket.");

        }
    }
}
