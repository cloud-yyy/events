using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence;

public class ApplicationDesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_DEVELOPMENT"),
				b => b.MigrationsAssembly("Persistence"))
			.UseSnakeCaseNamingConvention();

		return new ApplicationDbContext(builder.Options);
	}
}
