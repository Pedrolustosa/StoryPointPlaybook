using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Queries;

public record GetRoomStatisticsQuery(Guid RoomId) : IRequest<RoomStatisticsDto>;
