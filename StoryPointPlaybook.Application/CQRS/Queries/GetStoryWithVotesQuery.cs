using MediatR;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Application.CQRS.Queries;

public record GetStoryWithVotesQuery(Guid StoryId) : IRequest<Story>;
