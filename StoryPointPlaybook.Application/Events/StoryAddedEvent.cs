using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.Events;

public record StoryAddedEvent(string RoomCode, StoryResponse Story) : INotification;
