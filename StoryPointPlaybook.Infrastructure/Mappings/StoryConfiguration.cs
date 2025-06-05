using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
               .HasMaxLength(200)
               .IsRequired();

        builder.HasMany(s => s.Votes)
               .WithOne(v => v.Story)
               .HasForeignKey(v => v.StoryId);
    }
}
