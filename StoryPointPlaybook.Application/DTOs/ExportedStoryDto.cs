using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.DTOs
{
    public class ExportedStoryDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<VoteEntryDto> Votes { get; set; }
        public string Average { get; set; }
    }
}
