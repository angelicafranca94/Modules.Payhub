using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;


public partial class ComponentsAmount
{
    [JsonPropertyName("original")]
    public Original? Original { get; set; }

    [JsonPropertyName("saque")]
    public Loot? Loot { get; set; }

    [JsonPropertyName("troco")]
    public Change? Change { get; set; }

    [JsonPropertyName("juros")]
    public Fees? Fees { get; set; }

    [JsonPropertyName("multa")]
    public Fine? Fine { get; set; }

    [JsonPropertyName("abatimento")]
    public Reduction? Reduction { get; set; }

    [JsonPropertyName("desconto")]
    public Discount? Discount { get; set; }
}
