using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddInfrastructureConsumerLayer;

public static partial class AddInfrastructureConsumerLayerExtensions
{
    //repositories
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IDebtRepository, DebtRepository>()
                .AddScoped<IPixControlRepository, PixControlRepository>()
                .AddScoped<IBOControlRepository, BOControlRepository>()
                .AddScoped<IBMRemessaRepository, BMRemessaRepository>()
                .AddScoped<IWebhookItauBolecodePixRepository, WebhookItauBolecodePixRepository>();


    public static IServiceCollection AddConsumerInfrastructure(this IServiceCollection services) => services
      .AddRepositories();
}
