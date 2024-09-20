using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Modules.PayHub.CrossCutting.DependencyInjection.Extensions;

public static partial class DependencyInjectionExtensions
{
    public static IServiceCollection AddFromAbstraction<TAbstraction>(this IServiceCollection services, Assembly assembly)
    {
        var concreteImplementationsToInject = assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && type.IsClass && typeof(TAbstraction).IsAssignableFrom(type));

        foreach (var implementation in concreteImplementationsToInject)
            services.AddScoped(typeof(TAbstraction), implementation);

        return services;
    }
}