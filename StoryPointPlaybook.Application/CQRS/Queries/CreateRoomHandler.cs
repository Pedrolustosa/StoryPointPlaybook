using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.CQRS.Rooms.Handlers;

public class CreateRoomHandler : IRequestHandler<CreateRoomCommand, RoomDto>
{
    private readonly IRoomRepository _roomRepository;

    public CreateRoomHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room(request.Name, request.Scale, request.TimeLimit, request.AutoReveal);
        await _roomRepository.AddAsync(room);

        return new RoomDto
        {
            Id = room.Id,
            Code = room.Code,
            Name = room.Name
        };
    }
}
