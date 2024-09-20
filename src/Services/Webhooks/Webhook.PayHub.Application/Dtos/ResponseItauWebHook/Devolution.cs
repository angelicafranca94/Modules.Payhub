using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public class Devolution
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("rtrId")]
    public string RtrId { get; set; }

    [JsonPropertyName("valor")]
    public string? Amount { get; set; }

    [JsonPropertyName("natureza")]
    public string? Nature { get; set; }

    [JsonPropertyName("descricao")]
    public string? Description { get; set; }

    [JsonPropertyName("horario")]
    public Time? Time { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("motivo")]
    public string? Reason { get; set; }
}
