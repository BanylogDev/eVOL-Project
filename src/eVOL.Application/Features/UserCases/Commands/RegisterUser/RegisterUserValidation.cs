using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.RegisterUser
{
    public class RegisterUserValidation : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidation()
        {
            RuleFor(u => u.Dto.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters.");
            RuleFor(u => u.Dto.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
            RuleFor(u => u.Dto.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");

        }
    }
}
