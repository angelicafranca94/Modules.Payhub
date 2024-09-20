using Consumers.Service.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddApplicationLayer;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddApplicationWebhookLayer;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddInfrastructureLayer;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddInfrastructureWebhookLayer;

namespace Modules.PayHub.CrossCutting.DependencyInjection;

public static class Bootstrap
{

    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .RegisterServices(configuration)
            .AddMassTransit(busConfiguration =>
            {
                busConfiguration.AddConsumer<ItauPixDebtPaymentConsumer>();

                busConfiguration.UsingRabbitMq((context, configuration) =>
                {
                    services.SetupRabbitMqConnection(configuration);

                    configuration.ReceiveEndpoint("payhub_itauwebhook_queue", endpoint =>
                    {
                        endpoint.ConfigureConsumer<ItauPixDebtPaymentConsumer>(context);
                    });
                });
            });

        return services;
    }

    public static IServiceCollection AddApis(this IServiceCollection services, IConfiguration configuration) =>
        services.RegisterServices(configuration);

    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddEfCoreContexts(configuration)
            .AddPixInfrastructure()
            .AddPixApplication()
            .AddSharedSettings();

        return services;
    }

    public static IServiceCollection AddWebhook(this IServiceCollection services, IConfiguration configuration) =>
       services.RegisterServicesWebhook(configuration)
           .AddRabbitMqConnection();

    public static IServiceCollection RegisterServicesWebhook(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddEFCoreWebhookContext(configuration)
            .AddWebhookInfrastructure()
            .AddWebhookApplication()
            .AddSharedSettings();

        return services;
    }
}