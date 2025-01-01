using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.DateOfBirth)
            .IsRequired();

        builder
            .HasOne(b => b.Role)
            .WithMany();

        builder
            .HasMany(u => u.EventsParticipatedIn)
            .WithMany(e => e.Participants)
            .UsingEntity<Registration>(cjet => cjet.ToTable("registrations"));
    }
}
