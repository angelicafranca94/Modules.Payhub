using Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

namespace Webhook.PayHub.Application.Interfaces.Services;
public interface IWebhookItauListenerTreatmentService
{
    Task PublishWebhookResponseAsync(ItauWebhookDto webhookResponse);
}
