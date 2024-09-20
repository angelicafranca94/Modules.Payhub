using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public partial class Original
{
    [JsonPropertyName("valor")]
    public string? Amount { get; set; }
}
