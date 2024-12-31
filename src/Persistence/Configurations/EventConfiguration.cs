using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Place)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Category)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(e => e.MaxParticipants)
            .IsRequired()
            .HasAnnotation("CheckConstraint", "max_participants > 0");

        builder
            .HasOne(e => e.Image)
            .WithOne();
    }
}
