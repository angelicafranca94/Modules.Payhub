using Microsoft.AspNetCore.Diagnostics;
using Modules.PayHub.CrossCutting.DependencyInjection;
using System.Reflection;
using Webhook.PayHub.Application.Dtos.ResponseItauWebHook;
using Webhook.PayHub.Application.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory();

var configuration = builder.Configuration
    .SetBasePath(path)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddWebhook(configuration.Build());

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Homolog"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature is not null)
        {
            logger.LogError(contextFeature.Error, "Error: {Error}", contextFeature.Error);

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            });
        }
    });
});

var webhookGroup = app.MapGroup("/webhook_itau");

webhookGroup.MapPost("", async (ItauWebhookDto itauWebhookDto, IWebhookItauListenerTreatmentService _listenerService) =>
{
    await _listenerService.PublishWebhookResponseAsync(itauWebhookDto);
    return Results.Created("", itauWebhookDto);
})
.WithName("PostWebhook")
.WithOpenApi(x => new Microsoft.OpenApi.Models.OpenApiOperation(x)
{
    Summary = "Ouvinte dos json de retorno de pagamento Bolecode/PIX enviadas pelo Itaú.",
    Description = "Adiciona as informações e o json de retorno dos pagamentos vindos do Itaú, na base FIAP e envia algumas informações do json para a fila no RabbitMQ para o processamento da baixa pelo Consumer"
});

webhookGroup.MapGet("/hello", () =>
{
    return Results.Ok("The Webhook API is already up and running");
})
.WithName("GetWebhook")
.WithOpenApi(x => new Microsoft.OpenApi.Models.OpenApiOperation(x)
{
    Summary = "Rota para checar se a aplicação está no ar",
});

app.Run();