using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public class WebhookItauBolecodePixDto
{
    [JsonPropertyName("endToEndId")]
    public string EndToEndId { get; set; }

    [JsonPropertyName("txid")]
    public string? Txid { get; set; }

    [JsonPropertyName("valor")]
    public decimal Amount { get; set; }

    [JsonPropertyName("horario")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("infoPagador")]
    public string? InfoPay { get; set; }

    [JsonPropertyName("chave")]
    public string? Key { get; set; }

    [JsonPropertyName("componentesValor")]
    public ComponentsAmount? ComponentesAmount { get; set; }

    [JsonPropertyName("devolucoes")]
    public IEnumerable<Devolution>? Devolutions { get; set; }

}
