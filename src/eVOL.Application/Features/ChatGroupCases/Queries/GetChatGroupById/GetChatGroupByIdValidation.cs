using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Queries.GetChatGroupById
{
    public class GetChatGroupByIdValidation : AbstractValidator<GetChatGroupByIdQuery>
    {
        public GetChatGroupByIdValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("ChatGroup Id is required.");
        }
    }
}
