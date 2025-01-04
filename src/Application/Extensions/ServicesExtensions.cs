using Application.Behaviors;
using Application.Options;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServicesExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<RefreshTokenOptionsSetup>();
        services.Configure<AwsOptions>(configuration.GetSection("AWS"));

        services.AddMediatR(configuration => 
        {
            configuration.RegisterServicesFromAssembly(
                typeof(AssemblyReference).Assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(AssemblyReference).Assembly,
            includeInternalTypes: true
        );

        services.AddScoped<LinkFactory>();
    }
}
