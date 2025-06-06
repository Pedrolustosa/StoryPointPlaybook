using MediatR;
using StoryPointPlaybook.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Queries
{
    public record GetRoomStatisticsQuery(Guid RoomId) : IRequest<RoomStatisticsDto>;

}
