using System.Text;
using Amazon.S3;
using Domain;
using Domain.Authentication;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Handlers;
using Infrastructure.Authorization.Requirements;
using Infrastructure.Email;
using Infrastructure.Options;
using Infrastructure.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
        services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["JwtToken:Issuer"],
                    ValidAudience = configuration["JwtToken:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtToken:SecretKey"]!)
                    )
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.Admin, policy => 
                policy.Requirements.Add(new AdminRequirement()));

        services.AddScoped<IAuthorizationHandler, AdminRequirementHandler>();

        services
            .AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:SenderName"])
            .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"));

        services.AddScoped<IEmailSender, FluentEmailSender>();

        services.AddScoped<IS3Client, AmazonAwsS3Client>();

        services.AddSingleton<IAmazonS3>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AwsOptions>>().Value;

            var config = new AmazonS3Config
            {
                ServiceURL = options.ServiceURL,
                ForcePathStyle = true
            };

            return new AmazonS3Client(options.AccessKey, options.SecretKey, config);
        });

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<RefreshTokenOptionsSetup>();
        services.Configure<AwsOptions>(configuration.GetSection("AWS"));
    }
}
