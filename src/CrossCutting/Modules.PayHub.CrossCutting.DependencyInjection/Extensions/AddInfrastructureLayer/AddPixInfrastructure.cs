
using Api.Pix.Application.Interfaces.HttpClients;
using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Interfaces;
using Api.Pix.Domain.Interfaces.Utils;
using Api.Pix.Infrastructure.HttpClients;
using Api.Pix.Infrastructure.Repositories;
using Api.Pix.Infrastructure.Resiliences;
using Api.Pix.Infrastructure.Services;
using Api.Pix.Infrastructure.Settings;
using Api.Pix.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddInfrastructureLayer;

public static partial class AddInfrastructureLayerExtensions
{

    //Settings
    private static IServiceCollection AddSettings(this IServiceCollection services) => services
        .AddBindedSettings<GenerateTokenPixSettings>()
        .AddBindedSettings<PixUriSettings>()
        .AddBindedSettings<KeyForDecryptCodeDebtSettings>()
        .AddBindedSettings<PixSettings>()
        .AddBindedSettings<MemoryCachedSettings>()
        .AddBindedSettings<ResilienceSettings>()
        .AddBindedSettings<CorsSettings>();

    //repositories
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IDebtRepository, DebtRepository>()
                .AddScoped<IPixControlRepository, PixControlRepository>()
                .AddScoped<IAccountsBankRepository, AccountsBankRepository>()
                .AddTransient<IPixControlLogErrorRepository, PixControlLogErrorRepository>();

    //utils
    private static IServiceCollection AddUtils(this IServiceCollection services) => services
        .AddScoped<IRsaDecryptorUtil, RsaDecryptorUtil>();

    //AddClientDependencies
    private static IServiceCollection AddClientDependencies(this IServiceCollection services)
    {
        services.AddTransient<IItauPixClient, ItauPixClient>();
        services.AddTransient<ICertificateService, CertificateService>();
        services.AddSingleton<IResiliencePolicy, ResiliencePolicy>();
        services.AddTransient<IExternalApiService, ExternalApiService>();
        services.AddHttpContextAccessor();


        return services;
    }

    public static IServiceCollection AddPixInfrastructure(this IServiceCollection services) => services

        .AddSettings()
        .AddMemoryCache()
        .AddRepositories()
        .AddUtils()
        .AddClientDependencies();
}