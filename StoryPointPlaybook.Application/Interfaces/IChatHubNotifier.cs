using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.Interfaces
{
    public interface IChatHubNotifier
    {
        Task NotifyMessageSent(Guid roomId, string userName, string message, DateTime timestamp);
    }

}
