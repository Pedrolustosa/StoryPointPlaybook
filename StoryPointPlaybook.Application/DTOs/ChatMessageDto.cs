using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.DTOs
{
    public class ChatMessageDto
    {
        public string User { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }

}
