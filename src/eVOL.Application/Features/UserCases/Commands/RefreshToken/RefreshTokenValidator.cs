using FluentValidation;

namespace eVOL.Application.Features.UserCases.Commands.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(u => u.Dto.AccessToken)
                .NotEmpty()
                .WithMessage("Access token is required.");

            RuleFor(u => u.Dto.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");
        }
    }
}
