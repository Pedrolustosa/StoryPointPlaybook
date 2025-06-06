using MediatR;
using StoryPointPlaybook.Application.DTOs;

public record ExportRoomResultQuery(Guid RoomId) : IRequest<ExportResultDto>;
