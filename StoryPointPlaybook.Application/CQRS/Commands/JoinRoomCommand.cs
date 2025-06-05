using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Rooms.Commands;

public record JoinRoomCommand(string RoomCode, string DisplayName, string Role) : IRequest<UserDto>;
