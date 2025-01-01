using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Options;

public class JwtOptionsSetup(IConfiguration _configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtToken";

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);

        var key = $"{SectionName}:LifetimeInMinutes";
        options.Lifetime = TimeSpan.FromMinutes(int.Parse(_configuration[key]!));

        options.SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY")!;
    }
}
