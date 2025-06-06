using StoryPointPlaybook.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.Services
{
    public class InMemoryRateLimitService : IRateLimitService
    {
        private readonly Dictionary<string, Queue<DateTime>> _requests = new();
        private readonly object _lock = new();

        public bool CanProceed(string userKey, int limit, TimeSpan window)
        {
            lock (_lock)
            {
                if (!_requests.ContainsKey(userKey))
                    _requests[userKey] = new Queue<DateTime>();

                var queue = _requests[userKey];
                var now = DateTime.UtcNow;

                while (queue.Count > 0 && (now - queue.Peek()) > window)
                    queue.Dequeue();

                if (queue.Count >= limit)
                    return false;

                queue.Enqueue(now);
                return true;
            }
        }
    }

}
