using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.PayHub.CrossCutting.DependencyInjection;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddApplicationConsumerLayer;
using Modules.PayHub.CrossCutting.DependencyInjection.Extensions.AddInfrastructureConsumerLayer;
using System.Reflection;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory();

await Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging => logging.AddConsole())
    .ConfigureAppConfiguration((hostContext, configuration) =>
    {
        hostContext.HostingEnvironment.EnvironmentName = environment!;

        configuration.SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })

    .ConfigureServices((hostContext, services) => services
    .AddConsumers(hostContext.Configuration)
    .AddEfCoreConsumerContexts(hostContext.Configuration)
    .AddConsumerApplication()
    .AddConsumerInfrastructure())
    .RunConsoleAsync();