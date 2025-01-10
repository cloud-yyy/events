using Application.Extensions;
using Domain;
using Domain.Authentication;
using Domain.Constants;
using Domain.Entities;
using DotNetEnv;
using Infrastructure.Extensions;
using Persistence;
using Persistence.Extensions;
using Presentation.Extensions;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddPersistense(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await ConfigureDatabase(scope);
    await ConfigureS3Client(scope);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "events");
});

app.Run();

async Task ConfigureDatabase(IServiceScope scope)
{
    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        var admin = new User
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@mail.com",
            PasswordHash = scope.ServiceProvider
                .GetRequiredService<IPasswordHasher>().HashPassword("admin"),
            DateOfBirth = DateTime.UtcNow,
            Role = context.Roles.Single(r => r.Name == RoleNames.Admin)
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}

async Task ConfigureS3Client(IServiceScope scope)
{
    var amazonS3 = scope.ServiceProvider.GetRequiredService<IS3Client>();
    await amazonS3.EnsureBucketExistsAsync(builder.Configuration["AWS:BucketName"]!);
}