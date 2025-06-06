using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }


}
