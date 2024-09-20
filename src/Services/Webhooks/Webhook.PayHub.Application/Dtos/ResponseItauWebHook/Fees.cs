using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;


public partial class Fees
{
    [JsonPropertyName("valor_juros")]
    public string? InterestValue { get; set; }
}