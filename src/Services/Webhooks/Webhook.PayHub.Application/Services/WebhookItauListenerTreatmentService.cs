using AutoMapper;
using CrossCutting.PayHub.Shared.Constants;
using CrossCutting.PayHub.Shared.Dtos.Messages;
using CrossCutting.PayHub.Shared.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Webhook.PayHub.Application.Dtos.ResponseItauWebHook;
using Webhook.PayHub.Application.Interfaces.Repositories;
using Webhook.PayHub.Application.Interfaces.Services;
using Webhook.PayHub.Domain.Models;

namespace Webhook.PayHub.Application.Services;

public class WebhookItauListenerTreatmentService : IWebhookItauListenerTreatmentService
{
    private readonly ILogger<WebhookItauListenerTreatmentService> _logger;
    private readonly IWebhookItauBolecodePixRepository _webhookItauBolecodePixRepository;
    private readonly IWebhookItauBolecodePixLogErrorRepository _webhookItauBolecodePixLogErrorRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public WebhookItauListenerTreatmentService(ILogger<WebhookItauListenerTreatmentService> logger,
        IWebhookItauBolecodePixRepository webhookItauBolecodePixRepository,
        IWebhookItauBolecodePixLogErrorRepository webhookItauBolecodePixLogErrorRepository,
        IMapper mapper,
        IPublishEndpoint publishEndpoint
    )
    {
        _logger = logger;
        _webhookItauBolecodePixRepository = webhookItauBolecodePixRepository;
        _webhookItauBolecodePixLogErrorRepository = webhookItauBolecodePixLogErrorRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishWebhookResponseAsync(ItauWebhookDto webhookResponse)
    {
        try
        {
            _logger.LogInformation(LoggerConstants.StartService, nameof(WebhookItauListenerTreatmentService), nameof(PublishWebhookResponseAsync));

            if (webhookResponse is null)
                throw new ReturnWebhookException(ErrorsConstants.ReturnWebhookError);

            var webhookObject = webhookResponse.Pix.FirstOrDefault();

            var modelPixObjects = _mapper.Map<WebhookItauBolecodePixModel>(webhookObject);

            _logger.LogInformation("JSON WEBHOOK {webhookResponse}", JsonSerializer.Serialize(webhookResponse));

            modelPixObjects.JsonWebhook = JsonSerializer.Serialize(webhookResponse);

            _logger.LogInformation("Insert Json in WebhookItauBolecodePix...");

            //Por conta da time zone do itau, tem que reduzir 3 horas para igualar no nosso servidor
            var paymentData = modelPixObjects.Timestamp.AddHours(-3);

            modelPixObjects.Timestamp = paymentData;

            await _webhookItauBolecodePixRepository.InsertAsync(modelPixObjects);

            using var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(120));

            _logger.LogInformation("Sending message to queue...");

            await _publishEndpoint.Publish(new ItauWebhookMessage
            {
                Amount = webhookObject.Amount.ToString(),
                EndToEndId = webhookObject.EndToEndId,
                Timestamp = webhookObject.Timestamp.AddHours(-3).ToString(),
                InfoPay = webhookObject.InfoPay,
                Key = webhookObject.Key,
                Txid = webhookObject.Txid

            }, cancellationToken.Token);

            _logger.LogInformation("Message to queue has been sending");

            _logger.LogInformation(LoggerConstants.EndService, nameof(WebhookItauListenerTreatmentService), nameof(PublishWebhookResponseAsync));
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(LoggerConstants.ErrorLog, nameof(WebhookItauListenerTreatmentService), nameof(PublishWebhookResponseAsync), ErrorsConstants.PublishQueueTimeOut);
            await InsertLogErrorAsync(ex, webhookResponse);

            throw new OperationCanceledException(ErrorsConstants.PublishQueueTimeOut);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggerConstants.ErrorLog, nameof(WebhookItauListenerTreatmentService), nameof(PublishWebhookResponseAsync), ex.Message);
            await InsertLogErrorAsync(ex, webhookResponse);
            throw;
        }

    }

    private async Task InsertLogErrorAsync(Exception ex, ItauWebhookDto webhookResponse)
    {
        JsonSerializer.Serialize(webhookResponse);

        var webhookError = new WebhookItauBolecodePixLogErrorModel
        {
           JsonInput = webhookResponse is not null ? JsonSerializer.Serialize(webhookResponse) : "Objeto webhookResponse nulo",
            ErrorMessage = ex.Message,
            StackTrace = ex.StackTrace,
            TransactionId = webhookResponse is not null ? webhookResponse.Pix.FirstOrDefault().Txid : "Objeto webhookResponse nulo"

        };

        await _webhookItauBolecodePixLogErrorRepository.InsertAsync(webhookError);
    }

}