using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.DeleteUser
{
    public class DeleteUserValidation : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidation()
        {
            RuleFor(u => u.Dto.Id)
                .NotEmpty()
                .WithMessage("User ID is required to delete a user.");

            RuleFor(u => u.Dto.Password)
                .Equal(u => u.Dto.ConfirmPassword)
                .WithMessage("Password's dont match.");

        }
    }
}
