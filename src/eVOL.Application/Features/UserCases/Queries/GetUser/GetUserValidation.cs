using FluentValidation;

namespace eVOL.Application.Features.UserCases.Queries.GetUser
{
    public class GetUserValidation : AbstractValidator<GetUserQuery>
    {
        public GetUserValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("User ID is required to retrieve user information.");
        }
    }
}
