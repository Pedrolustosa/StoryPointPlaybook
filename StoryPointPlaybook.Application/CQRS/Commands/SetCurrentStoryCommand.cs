using MediatR;
using StoryPointPlaybook.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Commands
{
    public record SetCurrentStoryCommand(Guid RoomId, Guid StoryId) : IRequest<StoryResponse>;
}
