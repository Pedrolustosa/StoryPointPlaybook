using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Enums;

namespace StoryPointPlaybook.Application.CQRS.Rooms.Commands;

public record CreateRoomCommand(string Name, VotingScale Scale, int TimeLimit, bool AutoReveal) : IRequest<RoomDto>;
