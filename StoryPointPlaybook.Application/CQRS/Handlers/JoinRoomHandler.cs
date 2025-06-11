using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class JoinRoomHandler(IRoomRepository roomRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<JoinRoomCommand, UserDto>
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UserDto> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByCodeAsync(request.RoomCode);
        if (room == null || room.IsClosed)
            throw new InvalidOperationException("Sala não encontrada ou encerrada.");

        var user = new User(request.DisplayName, request.Role, room.Id);
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            RoomId = user.RoomId
        };
    }
}

