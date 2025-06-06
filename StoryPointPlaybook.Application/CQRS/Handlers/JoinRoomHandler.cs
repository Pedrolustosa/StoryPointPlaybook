using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class JoinRoomHandler : IRequestHandler<JoinRoomCommand, UserDto>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;

    public JoinRoomHandler(IRoomRepository roomRepository, IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByCodeAsync(request.RoomCode);
        if (room == null || room.IsClosed)
            throw new InvalidOperationException("Sala não encontrada ou encerrada.");

        var user = new User(request.DisplayName, request.Role, room.Id);
        await _userRepository.AddAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            RoomId = user.RoomId
        };
    }
}
