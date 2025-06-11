using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class CreateRoomHandler(IRoomRepository roomRepo, IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomRepository _roomRepo = roomRepo;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room(
            name: request.Name,
            scale: request.Scale,
            timeLimitInSeconds: request.TimeLimit,
            autoReveal: request.AutoReveal
        );

        var user = new User(
            name: request.CreatedBy,
            role: "Moderator",
            roomId: room.Id
        );

        room.Participants.Add(user);

        await _roomRepo.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();

        return new RoomResponse
        {
            Id = room.Id,
            Code = room.Code,
            Name = room.Name,
            Scale = room.Scale,
            TimeLimit = room.TimeLimitInSeconds,
            AutoReveal = room.AutoReveal,
            CreatedBy = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role
            },
            Participants =
            [
                new() {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role
                }
            ]
        };
    }
}



