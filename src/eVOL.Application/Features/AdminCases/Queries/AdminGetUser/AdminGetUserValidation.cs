using FluentValidation;

namespace eVOL.Application.Features.AdminCases.Queries.AdminGetUser
{
    public class AdminGetUserValidation : AbstractValidator<AdminGetUserQuery>
    {
        public AdminGetUserValidation()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
        }
    }
}
