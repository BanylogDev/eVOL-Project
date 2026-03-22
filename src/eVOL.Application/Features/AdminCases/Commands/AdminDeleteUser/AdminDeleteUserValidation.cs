using FluentValidation;

namespace eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser
{
    public class AdminDeleteUserValidation : AbstractValidator<AdminDeleteUserCommand>
    {
        public AdminDeleteUserValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
