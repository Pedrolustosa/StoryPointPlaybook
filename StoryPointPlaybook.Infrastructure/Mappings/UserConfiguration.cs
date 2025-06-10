using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.Role)
               .HasMaxLength(20)
               .IsRequired();
    }
}
