namespace StoryPointPlaybook.Domain.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException() : base("Sala não encontrada.") {}
}
