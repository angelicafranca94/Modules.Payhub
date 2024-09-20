using Api.Pix.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.PayHub.CrossCutting.DependencyInjection;

public static class AddEFCoreContexts
{
    public static IServiceCollection AddEfCoreContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringEducacional = configuration.GetConnectionString("EducacionalDB")
            .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER"))
            .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("${DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));

        var connectionStringBaseEducacional = configuration.GetConnectionString("BaseEducacionalDB")
            .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER"))
            .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("${DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));

        //Pix
        services.AddDbContext<PixControlContext>(options =>
        {
            options.UseSqlServer(connectionStringBaseEducacional);
            options.UseLazyLoadingProxies(true);
        });

        services.AddDbContext<PixControlLogErrorContext>(options =>
        {
            options.UseSqlServer(connectionStringBaseEducacional);
            options.UseLazyLoadingProxies(true);
        });

        services.AddDbContext<FNDebtsContext>(options =>
        {
            options.UseSqlServer(connectionStringBaseEducacional);
            options.UseLazyLoadingProxies(true);
        });

        services.AddDbContext<AccountsBankContext>(options =>
        {
            options.UseSqlServer(connectionStringBaseEducacional);
            options.UseLazyLoadingProxies(true);
        });

        return services;
    }
}
