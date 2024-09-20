using Consumers.PayHub.Application.Interfaces.Services;
using Consumers.PayHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddApplicationConsumerLayer;

public static partial class AddApplicationConsumersLayerExtensions
{
    private static IServiceCollection AddServices(this IServiceCollection services) => services
    .AddScoped<IPixBolecodePaymentService, PixBolecodePaymentService>();


    public static IServiceCollection AddConsumerApplication(this IServiceCollection services) => services
        .AddServices();
}
