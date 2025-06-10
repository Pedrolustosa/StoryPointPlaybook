using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.User).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
    }
}