using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.DisplayName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.Role)
               .HasMaxLength(20)
               .IsRequired();
    }
}
