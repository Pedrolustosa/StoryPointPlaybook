using MediatR;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class RevealVotesHandler(
    IStoryRepository storyRepo,
    IVoteRepository voteRepo,
    IGameHubNotifier notifier,
    IUnitOfWork unitOfWork) : IRequestHandler<RevealVotesCommand, Unit>
{
    private readonly IStoryRepository _storyRepo = storyRepo;
    private readonly IVoteRepository _voteRepo = voteRepo;
    private readonly IGameHubNotifier _notifier = notifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(RevealVotesCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepo.GetByIdWithVotesAsync(request.StoryId) ?? throw new InvalidOperationException("História não encontrada");

        // Revela cada voto
        foreach (var vote in story.Votes)
        {
            vote.Reveal();
            await _voteRepo.UpdateAsync(vote);
        }

        // Atualiza a história (pode calcular média, status, etc.)
        story.RevealVotes();
        await _storyRepo.UpdateAsync(story);

        await _unitOfWork.SaveChangesAsync();

        var voteResults = story.Votes.Select(v => new VoteResultDto
        {
            UserName = v.User.Name,
            Value = v.Value,
            IsRevealed = v.IsRevealed
        });

        await _notifier.NotifyVotesRevealedWithResults(story.RoomId, voteResults);


        return Unit.Value;
    }
}
