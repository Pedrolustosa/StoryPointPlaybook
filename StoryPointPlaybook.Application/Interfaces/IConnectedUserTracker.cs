using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.Interfaces
{
    public interface IConnectedUserTracker
    {
        void AddUser(string connectionId, Guid roomId);
        void RemoveUser(string connectionId);
        int GetParticipantCount(Guid roomId);
    }

}
