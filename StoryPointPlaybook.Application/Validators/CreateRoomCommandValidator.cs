using FluentValidation;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.Validators.Rooms;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da sala é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.TimeLimit)
            .GreaterThan(0).WithMessage("O tempo limite deve ser maior que 0.");
    }
}
