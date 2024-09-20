using Api.Pix.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Modules.PayHub.API.Configurations;

public static class CorsConfigurations
{
    public static string CorsPolicyName = nameof(CorsPolicyName);

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        var corsSettings = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<CorsSettings>>()
            .Value;

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(corsSettings.Urls)
            );
        });

        return services;
    }
}
