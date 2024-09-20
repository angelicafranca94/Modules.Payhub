namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public class ItauWebhookModel
{
    public IEnumerable<WebhookItauBolecodePixModel> Pix { get; set; }
}
