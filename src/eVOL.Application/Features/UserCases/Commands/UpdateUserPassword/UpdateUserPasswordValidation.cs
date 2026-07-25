using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUserPassword
{
    public class UpdateUserPasswordValidation : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required for updating user information.");

            RuleFor(u => u.Dto.CurrentPassword)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");

            RuleFor(u => u.Dto.NewPassword)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .Equal(u => u.Dto.ConfirmNewPassword)
                .WithMessage("New Password and Confirm New Password must match.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");

            RuleFor(u => u.Dto.ConfirmNewPassword)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");
        }
    }

}
