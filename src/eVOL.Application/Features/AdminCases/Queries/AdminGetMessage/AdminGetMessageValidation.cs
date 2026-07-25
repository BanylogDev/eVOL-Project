using FluentValidation;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetMessage
{
    public class AdminGetMessageValidation : AbstractValidator<AdminGetMessageQuery>
    {
        public AdminGetMessageValidation()
        {
            RuleFor(m => m.id)
                .NotEmpty()
                .WithMessage("Message ID cannot be empty.");
        }
    }
}
