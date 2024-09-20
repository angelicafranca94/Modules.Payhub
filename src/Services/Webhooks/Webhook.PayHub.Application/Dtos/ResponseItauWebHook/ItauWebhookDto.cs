using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public class ItauWebhookDto
{
    [JsonPropertyName("pix")]
    public IEnumerable<WebhookItauBolecodePixDto> Pix { get; set; }
}
