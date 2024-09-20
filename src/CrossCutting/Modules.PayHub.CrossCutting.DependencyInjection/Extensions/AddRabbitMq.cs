using Consumers.Service.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Modules.PayHub.CrossCutting.DependencyInjection;

public static class AddRabbitMq
{
    public static IServiceCollection SetupRabbitMqConnection(this IServiceCollection services, IRabbitMqBusFactoryConfigurator configuration)
    {
        var rabbitMqSettings = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<RabbitMqSettings>>()
            .Value;

        configuration.Host(new Uri($"rabbitmq://{rabbitMqSettings.Host}:{rabbitMqSettings.Port}/%2F{rabbitMqSettings.VHost.Replace("${RABBIT_VHOST}", Environment.GetEnvironmentVariable("RABBIT_VHOST"))}"), host =>
        //configuration.Host(new Uri($"rabbitmq://{rabbitMqSettings.Host}:{rabbitMqSettings.Port}/{rabbitMqSettings.VHost.Replace("${RABBIT_VHOST}", Environment.GetEnvironmentVariable("RABBIT_VHOST"))}"), host =>
       {
           host.Username(rabbitMqSettings.User.Replace("${RABBIT_USER}", Environment.GetEnvironmentVariable("RABBIT_USER")));
           host.Password(rabbitMqSettings.Pass.Replace("${RABBIT_PASS}", Environment.GetEnvironmentVariable("RABBIT_PASS")));
       });

        return services;
    }

    public static IServiceCollection AddRabbitMqConnection(this IServiceCollection services)
    {
        services.AddMassTransit(busConfiguration =>
            busConfiguration.UsingRabbitMq((context, configuration) => services.SetupRabbitMqConnection(configuration))
        );

        return services;
    }
}
