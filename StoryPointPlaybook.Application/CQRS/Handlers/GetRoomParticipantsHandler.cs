using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class GetRoomParticipantsHandler(IUserRepository userRepository) : IRequestHandler<GetRoomParticipantsQuery, List<UserResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<UserResponse>> Handle(GetRoomParticipantsQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByRoomIdAsync(request.RoomId);

        return [.. users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Role = u.Role
        })];
    }
}
