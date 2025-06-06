using MediatR;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Handlers
{
    public class GetRoomParticipantsHandler : IRequestHandler<GetRoomParticipantsQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetRoomParticipantsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> Handle(GetRoomParticipantsQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByRoomIdAsync(request.RoomId);

            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role
            }).ToList();
        }
    }

}
