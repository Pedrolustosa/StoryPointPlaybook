using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class JoinRoomHandler(IUnitOfWork unitOfWork) : IRequestHandler<JoinRoomCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UserDto> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByCodeAsync(request.RoomCode);
        if (room == null || room.IsClosed)
            throw new InvalidOperationException("Sala não encontrada ou encerrada.");

        var user = new User(request.DisplayName, request.Role, room.Id);
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            RoomId = user.RoomId
        };
    }
}
