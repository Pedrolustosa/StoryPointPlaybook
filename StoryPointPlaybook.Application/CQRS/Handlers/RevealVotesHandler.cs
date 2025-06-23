using MediatR;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class RevealVotesHandler(
    IGameHubNotifier notifier,
    IUnitOfWork unitOfWork) : IRequestHandler<RevealVotesCommand, Unit>
{
    private readonly IGameHubNotifier _notifier = notifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(RevealVotesCommand request, CancellationToken cancellationToken)
    {
        var story = await _unitOfWork.Stories.GetByIdWithVotesAsync(request.StoryId) ?? throw new InvalidOperationException("História não encontrada");

        // Revela cada voto
        foreach (var vote in story.Votes)
        {
            vote.Reveal();
            await _unitOfWork.Votes.UpdateAsync(vote);
        }

        // Atualiza a história (pode calcular média, status, etc.)
        story.RevealVotes();
        await _unitOfWork.Stories.UpdateAsync(story);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
