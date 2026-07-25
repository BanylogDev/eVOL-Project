using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.UpdateUser
{
    public class UpdateUserNameValidation : AbstractValidator<UpdateUserNameCommand>
    {
        public UpdateUserNameValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required for updating user information.");

            RuleFor(u => u.Dto.NewName)
                .NotEmpty()
                .WithMessage("Username cannot be empty.")
                .MinimumLength(4)
                .WithMessage("Username must be bigger than 4 characters.")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters.");

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
