using FluentValidation;

namespace eVOL.Application.Features.AdminCases.Commands.AdminUnBanUser
{
    public class AdminUnBanUserValidation : AbstractValidator<AdminUnBanUserCommand>
    {
        public AdminUnBanUserValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required to delete a user.");
        }
    }
}
