using Microsoft.Extensions.DependencyInjection;
using Webhook.PayHub.Application.Interfaces.Services;
using Webhook.PayHub.Application.Services;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddApplicationWebhookLayer;

public static partial class AddApplicationWebhookLayerExtensions
{

    //services
    private static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddScoped<IWebhookItauListenerTreatmentService, WebhookItauListenerTreatmentService>();

    private static IServiceCollection AddWebhookAutoMapper(this IServiceCollection services) =>
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    public static IServiceCollection AddWebhookApplication(this IServiceCollection services) => services
        .AddServices()
        .AddWebhookAutoMapper();
}

