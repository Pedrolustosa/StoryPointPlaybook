using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.API.SignalR;

public class ConnectedUserTracker : IConnectedUserTracker
{
    private readonly Dictionary<string, Guid> _userConnections = new();
    private readonly object _lock = new();

    public void AddUser(string connectionId, Guid roomId)
    {
        lock (_lock)
        {
            _userConnections[connectionId] = roomId;
        }
    }

    public void RemoveUser(string connectionId)
    {
        lock (_lock)
        {
            _userConnections.Remove(connectionId);
        }
    }

    public int GetParticipantCount(Guid roomId)
    {
        lock (_lock)
        {
            return _userConnections.Values.Count(r => r == roomId);
        }
    }
}
