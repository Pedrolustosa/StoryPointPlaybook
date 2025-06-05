using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Infrastructure.Mappings;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Code)
               .HasMaxLength(6)
               .IsRequired();

        builder.HasMany(r => r.Participants)
               .WithOne(u => u.Room)
               .HasForeignKey(u => u.RoomId);

        builder.HasMany(r => r.Stories)
               .WithOne(s => s.Room)
               .HasForeignKey(s => s.RoomId);
    }
}
