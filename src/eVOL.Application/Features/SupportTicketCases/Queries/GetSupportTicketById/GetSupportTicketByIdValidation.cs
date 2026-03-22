using FluentValidation;

namespace eVOL.Application.Features.SupportTicketCases.Queries.GetSupportTicketById
{
    public class GetSupportTicketByIdValidation : AbstractValidator<GetSupportTicketByIdQuery>
    {
        public GetSupportTicketByIdValidation()
        {
            RuleFor(s => s.Id)
                .NotEmpty()
                .WithMessage("SupportTicket ID is required to retrieve a support ticket.");
        }
    }
}
