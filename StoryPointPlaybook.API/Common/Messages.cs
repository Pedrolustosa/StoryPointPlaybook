namespace StoryPointPlaybook.API.Common;

public static class Messages
{
    public static class Success
    {
        public const string MessageSent = "Mensagem enviada com sucesso.";
        public const string RoomCreated = "Sala criada com sucesso.";
        public const string UserJoined = "Usuário entrou na sala com sucesso.";
        public const string StoryAdded = "História adicionada com sucesso.";
        public const string VoteSubmitted = "Voto registrado com sucesso.";
        public const string VotesRevealed = "Votos revelados com sucesso.";
        public const string ParticipantsRetrieved = "Participantes recuperados com sucesso.";
        public const string CurrentStorySelected = "História definida para votação com sucesso.";
    }

    public static class Error
    {
        public const string MessageEmpty = "Mensagem não pode ser vazia.";
        public const string RoomNotFound = "Sala não encontrada.";
        public const string UserNotFound = "Usuário não encontrado.";
        public const string StoryNotFound = "História não encontrada.";
        public const string CurrentStoryNotFound = "História não encontrada ou não pertence à sala.";
    }
}