using Consumers.Service.Settings;
using CrossCutting.PayHub.Shared;
using CrossCutting.PayHub.Shared.ApiClients;
using CrossCutting.PayHub.Shared.ApiClients.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Modules.PayHub.CrossCutting.DependencyInjection;

public static partial class AddSharedLayerExtensions
{
    public static IServiceCollection AddSharedSettings(this IServiceCollection services)
    {
        services.AddBindedSettings<PixApiClientSettings>();
        services.AddHttpClient("PixApiClient", (serviceProvicer, client) =>
        {
            var settings = serviceProvicer.GetRequiredService<IOptions<PixApiClientSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
        });

        services.AddBindedSettings<RabbitMqSettings>();
        services.AddTransient<IPixApiClient, PixApiClient>();

        return services;
    }
}
