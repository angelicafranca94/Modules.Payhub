using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public partial class Change
{
    [JsonPropertyName("valor")]
    public string Amount { get; set; }

    [JsonPropertyName("modalidadeAgente")]
    public string AgentModality { get; set; }

    [JsonPropertyName("prestadorDeServicoDeSaque")]
    public string WithdrawalServiceProvider { get; set; }
}