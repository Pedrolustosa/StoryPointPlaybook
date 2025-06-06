using FluentValidation;

namespace StoryPointPlaybook.Application.Validators;

public class SendMessageValidator : AbstractValidator<(string User, string Message)>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("Usuário é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("A mensagem não pode estar vazia.")
            .MaximumLength(1000)
            .MinimumLength(1);
    }
}
