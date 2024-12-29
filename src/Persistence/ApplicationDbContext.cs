using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

#pragma warning disable CS8618
public class ApplicationDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Image> Images { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}
#pragma warning restore CS8618
