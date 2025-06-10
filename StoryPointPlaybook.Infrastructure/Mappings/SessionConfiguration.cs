using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
    }
}
