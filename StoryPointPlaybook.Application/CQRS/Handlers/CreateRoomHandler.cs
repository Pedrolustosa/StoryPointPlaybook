using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class CreateRoomHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomRepository _roomRepo;
    private readonly IUserRepository _userRepo;

    public CreateRoomHandler(IRoomRepository roomRepo, IUserRepository userRepo)
    {
        _roomRepo = roomRepo;
        _userRepo = userRepo;
    }

    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        // Criar sala
        var room = new Room(
            name: request.Name,
            scale: request.Scale,
            timeLimitInSeconds: request.TimeLimit,
            autoReveal: request.AutoReveal
        );

        // Criar usuário PO
        var user = new User(
            name: request.CreatedBy,
            role: "Moderator",
            roomId: room.Id
        );

        // Adiciona o PO aos participantes da sala
        room.Participants.Add(user);

        // Persiste ambos (EF salva User pela navegação)
        await _roomRepo.AddAsync(room); // SaveChanges incluirá o user via relacionamento

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
            Participants = new List<UserResponse>
            {
                new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role
                }
            }
        };
    }
}


