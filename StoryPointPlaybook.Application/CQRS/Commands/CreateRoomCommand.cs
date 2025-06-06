using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Enums;

namespace StoryPointPlaybook.Application.CQRS.Rooms.Commands;

public class CreateRoomCommand : IRequest<RoomResponse>
{
    public string Name { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
    public VotingScale Scale { get; set; }
    public int TimeLimit { get; set; }
    public bool AutoReveal { get; set; }
}
