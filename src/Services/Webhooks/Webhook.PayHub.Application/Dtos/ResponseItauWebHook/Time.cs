using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public partial class Time
{
    [JsonPropertyName("solicitacao")]
    public string Solicitation { get; set; }

    [JsonPropertyName("liquidacao")]
    public string Liquidation { get; set; }
}