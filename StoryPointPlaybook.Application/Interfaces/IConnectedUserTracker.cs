namespace StoryPointPlaybook.Application.Interfaces;

public interface IConnectedUserTracker
{
    void AddUser(string connectionId, Guid roomId);
    void RemoveUser(string connectionId);
    int GetParticipantCount(Guid roomId);
}
