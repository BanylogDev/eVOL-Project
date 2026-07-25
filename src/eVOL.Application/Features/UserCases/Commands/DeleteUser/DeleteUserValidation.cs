using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.DeleteUser
{
    public class DeleteUserValidation : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required to delete a user.");

            RuleFor(u => u.Dto.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .Equal(u => u.Dto.ConfirmPassword)
                .WithMessage("New Password and Confirm New Password must match.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");

            RuleFor(u => u.Dto.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(32)
                .WithMessage("Password cannot exceed 32 characters.");

        }
    }
}
