using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.HasData
        (
            new Role
            { 
                Id = new Guid("62e8f843-f88f-4c97-86ab-358eb2bf3e66"), 
                Name = RoleNames.Admin
            },
            new Role 
            { 
                Id = new Guid("94c2d1fa-7598-432e-a5b8-9ad619ecbe33"), 
                Name = RoleNames.User 
            }
        );
    }
}
