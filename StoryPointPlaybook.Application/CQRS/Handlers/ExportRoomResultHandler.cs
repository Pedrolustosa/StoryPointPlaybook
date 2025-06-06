using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers
{
    public class ExportRoomResultHandler : IRequestHandler<ExportRoomResultQuery, ExportResultDto>
    {
        private readonly IRoomRepository _roomRepo;

        public ExportRoomResultHandler(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<ExportResultDto> Handle(ExportRoomResultQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetByIdAsync(request.RoomId);
            if (room == null)
                throw new InvalidOperationException("Sala não encontrada.");

            var stories = room.Stories.Select(story =>
            {
                var voteValues = story.Votes.Select(v => v.Value).ToList();

                var avg = voteValues.All(v => double.TryParse(v, out _))
                    ? voteValues.Select(v => double.Parse(v)).Average().ToString("0.0")
                    : "-";

                return new ExportedStoryDto
                {
                    Title = story.Title,
                    Description = story.Description,
                    Votes = story.Votes.Select(v => new VoteEntryDto
                    {
                        User = v.User.Name,
                        Value = v.Value
                    }).ToList(),
                    Average = avg
                };
            }).ToList();

            return new ExportResultDto
            {
                RoomCode = room.Code,
                RoomName = room.Name,
                Stories = stories
            };
        }
    }
}