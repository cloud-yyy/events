using Application;
using Application.Behaviors;
using Ardalis.Result;
using Domain;
using Domain.Repositories;
using DotNetEnv;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
{
    options
        .UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_DEVELOPMENT"))
        .UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(Presentation.Controllers.ApiController).Assembly
);

builder.Services.AddMediatR(configuration => 
{
    configuration.RegisterServicesFromAssembly(
        typeof(Application.Events.CreateEvent.CreateEventCommandHandler).Assembly);
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddValidatorsFromAssembly(
    typeof(Application.Events.CreateEvent.CreateEventCommandValidator).Assembly,
    includeInternalTypes: true
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    context.Database.EnsureCreated();
}

app.MapControllers();

app.Run();
