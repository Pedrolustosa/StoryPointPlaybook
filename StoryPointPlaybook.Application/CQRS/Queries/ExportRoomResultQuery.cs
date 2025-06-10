using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Queries;

public record ExportRoomResultQuery(Guid RoomId) : IRequest<ExportResultDto>;