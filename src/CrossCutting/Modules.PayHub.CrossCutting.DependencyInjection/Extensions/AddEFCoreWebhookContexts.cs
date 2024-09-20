using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webhook.PayHub.Infrastructure.DBContexts;

namespace Modules.PayHub.CrossCutting.DependencyInjection;

public static class AddEFCoreWebhookContexts
{
    public static IServiceCollection AddEFCoreWebhookContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringBaseEducacional = configuration.GetConnectionString("BaseEducacionalDB")
            .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER"))
            .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("${DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));


        services.AddDbContext<PixWebhookItauContext>(options =>
        {
            options.UseSqlServer(connectionStringBaseEducacional);
            options.UseLazyLoadingProxies(true);
        });

        services.AddDbContext<WebhookItauBolecodePixLogErrorContext>(options =>
        {
            options.UseSqlServer(connectionStringBaseEducacional);
            options.UseLazyLoadingProxies(true);
        });

        return services;
    }
}
