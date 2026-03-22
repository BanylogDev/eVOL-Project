using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Commands.UnClaimSupportTicket
{
    public class UnClaimSupportTicketValidation : AbstractValidator<UnClaimSupportTicketCommand>
    {
        public UnClaimSupportTicketValidation()
        {
            RuleFor(s => s.Dto.Id)
                .NotEmpty()
                .WithMessage("SupportTicket ID is required."); 
            RuleFor(s => s.Dto.OpenedBy)
                .NotEmpty()
                .WithMessage("OpenedBy ID is required to unclaim the support ticket.");
        }
    }
}
