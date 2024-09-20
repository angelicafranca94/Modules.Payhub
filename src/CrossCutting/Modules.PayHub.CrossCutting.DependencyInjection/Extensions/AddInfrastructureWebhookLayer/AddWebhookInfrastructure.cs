using Microsoft.Extensions.DependencyInjection;
using Webhook.PayHub.Application.Interfaces.Repositories;
using Webhook.PayHub.Infrastructure.Repositories;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddInfrastructureWebhookLayer;

public static partial class AddInfrastructureWebhookLayerExtensions
{
    //repositories
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
      services
        .AddScoped<IWebhookItauBolecodePixRepository, WebhookItauBolecodePixRepository>()
        .AddScoped<IWebhookItauBolecodePixLogErrorRepository, WebhookItauBolecodePixLogErrorRepository>();

    public static IServiceCollection AddWebhookInfrastructure(this IServiceCollection services) => services
      .AddRepositories();
}
