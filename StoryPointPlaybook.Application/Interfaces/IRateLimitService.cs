using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.Interfaces
{
    public interface IRateLimitService
    {
        bool CanProceed(string userKey, int limit, TimeSpan window);
    }

}
