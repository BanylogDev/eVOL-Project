using FluentValidation;

namespace eVOL.Application.Features.AdminCases.Commands.AdminBanUser
{
    public class AdminBanUserValidation : AbstractValidator<AdminBanUserCommand>
    {
        public AdminBanUserValidation()
        {
            RuleFor(x => x.Dto.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.Dto.BannedUntil)
                .NotEmpty()
                .WithMessage("BannedUntil date is required.")
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("BannedUntil date must be in the future.");

            RuleFor(x => x.Dto.Reason)
                .MinimumLength(6)
                .WithMessage("Reason must be greater than 6 characters.")
                .MaximumLength(250)
                .WithMessage("Reason cannot exceed 250 characters.");
        }
    }
}
