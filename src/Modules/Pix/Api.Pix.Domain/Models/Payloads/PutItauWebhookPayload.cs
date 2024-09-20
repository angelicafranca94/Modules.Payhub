using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.Payloads;
public class PutItauWebhookPayload
{
    [JsonPropertyName("webhookUrl")]
    public string WebhookUrl { get; set; }
}
