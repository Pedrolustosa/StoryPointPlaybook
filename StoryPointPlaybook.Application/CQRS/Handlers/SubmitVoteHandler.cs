using MediatR;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Events;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SubmitVoteHandler(
    IUnitOfWork unitOfWork,
    IGameHubNotifier gameHubNotifier,
    IPublisher publisher) : IRequestHandler<SubmitVoteCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGameHubNotifier _gameHubNotifier = gameHubNotifier;
    private readonly IPublisher _publisher = publisher;

    public async Task<bool> Handle(SubmitVoteCommand request, CancellationToken cancellationToken)
    {
        // Buscar a story com a room para validação
        var story = await _unitOfWork.Stories.GetByIdWithRoomAsync(request.StoryId);
        if (story == null)
            return false;

        // Buscar o usuário
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
            return false;

        // Verificar se o usuário pertence à mesma room da story
        if (user.RoomId != story.RoomId)
            return false;

        // Verificar se já existe um voto para este usuário e story
        var existingVote = await _unitOfWork.Votes.GetByUserAndStoryAsync(request.UserId, request.StoryId);

        if (existingVote != null)
        {
            // Atualizar voto existente
            existingVote.SetValue(request.Value);
        }
        else
        {
            // Criar novo voto
            var newVote = new Vote(
                storyId: request.StoryId,
                userId: request.UserId,
                value: request.Value
            );

            await _unitOfWork.Votes.AddAsync(newVote);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Notificar que o usuário votou
        await _gameHubNotifier.NotifyUserVoted(story.RoomId, request.UserId);

        // Verificar se todos os participantes da room votaram (excluindo POs)
        var roomParticipants = await _unitOfWork.Users.GetByRoomIdAsync(story.RoomId);
        var voters = roomParticipants.Where(p => !string.Equals(p.Role, "PO", StringComparison.OrdinalIgnoreCase)).ToList();
        var storyVotes = await _unitOfWork.Votes.GetByStoryIdAsync(request.StoryId);

        var allVoted = voters.All(participant =>
            storyVotes.Any(vote => vote.UserId == participant.Id));

        // Se todos votaram e auto-reveal está ativado, revelar os votos
        if (allVoted && story.Room.AutoReveal)
        {
            story.RevealVotes();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _gameHubNotifier.NotifyVotesRevealed(story.RoomId);
        }

        // Publicar evento de domínio
        await _publisher.Publish(new VoteSubmittedEvent(request.StoryId, request.UserId, story.VotesRevealed), cancellationToken);

        // Obter status de votação atualizado (apenas para votantes, excluindo POs)
        var votingStatusList = voters.Select(participant =>
        {
            var hasVoted = storyVotes.Any(vote => vote.UserId == participant.Id);
            return new VotingStatusDto
            {
                UserId = participant.Id,
                DisplayName = participant.Name,
                HasVoted = hasVoted,
                VoteValue = hasVoted && story.VotesRevealed
                    ? storyVotes.First(v => v.UserId == participant.Id).Value
                    : null
            };
        }).ToList();

        // Notificar status de votação atualizado
        await _gameHubNotifier.NotifyVotingStatusUpdated(story.RoomId, votingStatusList);

        return true;
    }
}