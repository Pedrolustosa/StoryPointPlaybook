using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.DTOs
{
    public class SendMessageRequest
    {
        public string UserName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }

}
