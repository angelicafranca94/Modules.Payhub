using Microsoft.Extensions.DependencyInjection;

namespace Modules.PayHub.CrossCutting.DependencyInjection;

public static partial class DependencyInjectionExtensions
{
    public static IServiceCollection AddBindedSettings<TSettings>(this IServiceCollection services) where TSettings : class
    {
        services
           .AddOptions<TSettings>()
           .BindConfiguration(typeof(TSettings).Name)
           .ValidateDataAnnotations();

        return services;
    }
}
