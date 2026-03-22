using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.LoginUser
{
    public class LoginUserValidation : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidation()
        {
            RuleFor(u => u.Dto.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
            RuleFor(u => u.Dto.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");
        }
    }
}
