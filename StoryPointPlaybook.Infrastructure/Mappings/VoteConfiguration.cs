using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Value)
               .HasMaxLength(20)
               .IsRequired();

        builder.HasOne(v => v.User)
               .WithMany(u => u.Votes)
               .HasForeignKey(v => v.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}