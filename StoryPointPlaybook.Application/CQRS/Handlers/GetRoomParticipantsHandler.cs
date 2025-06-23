using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class GetRoomParticipantsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoomParticipantsQuery, List<UserResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<UserResponse>> Handle(GetRoomParticipantsQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetByRoomIdAsync(request.RoomId);

        return users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Role = u.Role
        }).ToList();
    }
}
