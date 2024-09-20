using Api.Pix.Application.Interfaces.CacheServices;
using Api.Pix.Application.Interfaces.Services;
using Api.Pix.Application.Services;
using Api.Pix.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddApplicationLayer;
public static partial class AddApplicationLayerExtensions
{
    private static IServiceCollection AddPixServices(this IServiceCollection services) => services
        .AddScoped<IQRCodeService, QRCodeService>()
        .AddScoped<IInMemoryCachedTokenService, InMemoryCachedTokenService>();

    private static IServiceCollection AddPixLogging(this IServiceCollection services) => services
        .AddLogging(configure =>
        {
            configure.AddConsole();
            configure.AddDebug();
        });

    private static IServiceCollection AddPixAutoMapper(this IServiceCollection services) =>
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    public static IServiceCollection AddPixApplication(this IServiceCollection services) => services
        .AddPixServices()
        .AddPixLogging()
        .AddPixAutoMapper();
}