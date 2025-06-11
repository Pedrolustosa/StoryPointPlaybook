namespace StoryPointPlaybook.Domain.Exceptions;

public class StoryNotFoundException : Exception
{
    public StoryNotFoundException() : base("História não encontrada ou não pertence à sala.") {}
}
