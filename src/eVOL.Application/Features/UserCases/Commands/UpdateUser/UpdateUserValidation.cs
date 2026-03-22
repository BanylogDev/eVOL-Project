using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUser
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidation()
        {
            RuleFor(u => u.Dto.Id)
                .NotEmpty()
                .WithMessage("User ID is required for updating user information.");

            RuleFor(u => u.Dto.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(255)
                .WithMessage("Email cannot exceed 255 characters.");

            RuleFor(u => u.Dto.Name)
                .NotEmpty()
                .WithMessage("Username cannot be empty.")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters.");

            RuleFor(u => u.Dto.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .Equal(u => u.Dto.ConfirmPassword)
                .WithMessage("Password and Confirm Password must match.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");
        }
    }
}
