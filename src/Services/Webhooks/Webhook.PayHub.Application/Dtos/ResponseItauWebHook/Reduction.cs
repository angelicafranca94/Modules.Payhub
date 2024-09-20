using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;

public partial class Reduction
{
    [JsonPropertyName("valor_abatimento_documento_cobranca_pix")]
    public string? ValueRebateDocumentBillingPix { get; set; }
}