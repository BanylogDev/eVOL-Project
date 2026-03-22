using FluentValidation;

namespace eVOL.Application.Features.ChatGroupCases.Commands.CreateChatGroup
{
    public class CreateChatGroupValidator : AbstractValidator<CreateChatGroupCommand>
    {
        public CreateChatGroupValidator()
        {
            RuleFor(c => c.Dto.OwnerId)
                .NotEmpty()
                .WithMessage("OwnerId is required.");

            RuleFor(c => c.Dto.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.");

             RuleFor(c => c.Dto.TotalUsers)
                .GreaterThan(0)
                .WithMessage("TotalUsers must be greater than 0.");

             RuleFor(c => c.Dto.GroupUsers)
                .NotEmpty()
                .WithMessage("GroupUsers is required.");


        }
    }
}
