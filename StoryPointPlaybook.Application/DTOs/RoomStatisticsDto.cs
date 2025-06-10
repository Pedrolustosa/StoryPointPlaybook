namespace StoryPointPlaybook.Application.DTOs;

public class RoomStatisticsDto
{
    public int TotalStories { get; set; }
    public int TotalVotes { get; set; }
    public int DistinctUsers { get; set; }
    public double AvgVotesPerStory { get; set; }
    public double ConsensusRate { get; set; }
}
