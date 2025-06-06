using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Commands
{
    public record SendMessageCommand(Guid RoomId, string UserName, string Message) : IRequest<Unit>;

}
