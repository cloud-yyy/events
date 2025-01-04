using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Options;

public class JwtOptionsSetup(IConfiguration _configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtToken";

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);

        options.Lifetime = TimeSpan.FromMinutes(
            int.Parse(_configuration[$"{SectionName}:LifetimeInMinutes"]!)
        );
    }
}
