using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Options;

public class RefreshTokenOptionsSetup(
    IConfiguration _configuration
) : IConfigureOptions<RefreshTokenOptions>
{
    private const string SectionName = "RefreshToken";

    public void Configure(RefreshTokenOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);

        var key = $"{SectionName}:LifetimeInDays";
        options.Lifetime = TimeSpan.FromDays(int.Parse(_configuration[key]!));
    }
}
