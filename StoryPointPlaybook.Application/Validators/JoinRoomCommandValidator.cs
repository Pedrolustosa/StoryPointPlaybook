using FluentValidation;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.Validators.Rooms;

public class JoinRoomCommandValidator : AbstractValidator<JoinRoomCommand>
{
    public JoinRoomCommandValidator()
    {
        RuleFor(x => x.RoomCode)
            .NotEmpty().WithMessage("O código da sala é obrigatório.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Role)
            .Must(role => role == "Moderator" || role == "Participant")
            .WithMessage("Role inválida. Use 'Moderator' ou 'Participant'.");
    }
}
