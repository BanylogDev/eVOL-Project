using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUserEmail
{
    public class UpdateUserEmailValidation : AbstractValidator<UpdateUserEmailCommand>
    {
        public UpdateUserEmailValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required for updating user information.");

            RuleFor(u => u.Dto.NewEmail)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(255)
                .WithMessage("Email cannot exceed 255 characters.");

            RuleFor(u => u.Dto.CurrentPassword)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");
        }
    }
}
