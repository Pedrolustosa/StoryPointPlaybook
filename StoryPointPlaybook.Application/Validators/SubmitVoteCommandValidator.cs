using FluentValidation;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Application.Validators.Stories;

public class SubmitVoteCommandValidator : AbstractValidator<SubmitVoteCommand>
{
    public SubmitVoteCommandValidator()
    {
        RuleFor(x => x.StoryId)
            .NotEqual(Guid.Empty).WithMessage("História inválida.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("Usuário inválido.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Valor do voto obrigatório.")
            .MaximumLength(20);
    }
}
