using System.Text;
using Application;
using Application.Abstractions;
using Application.Behaviors;
using Domain;
using Domain.Repositories;
using DotNetEnv;
using FluentValidation;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Handlers;
using Infrastructure.Authorization.Requirements;
using Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Repositories;
using Web.Middlewares;

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
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

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

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
builder.Services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<RefreshTokenOptionsSetup>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtToken:Issuer"],
            ValidAudience = builder.Configuration["JwtToken:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")!)
            )
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(PolicyNames.Admin, policy => 
        policy.Requirements.Add(new AdminRequirement()))
    .AddPolicy(PolicyNames.SameUser, policy => 
        policy.Requirements.Add(new SameUserRequirement()));

builder.Services.AddScoped<IAuthorizationHandler, AdminRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SameUserRequirementHandler>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    context.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
