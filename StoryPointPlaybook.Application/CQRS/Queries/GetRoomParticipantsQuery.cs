using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Queries;

public record GetRoomParticipantsQuery(Guid RoomId) : IRequest<List<UserResponse>>;
