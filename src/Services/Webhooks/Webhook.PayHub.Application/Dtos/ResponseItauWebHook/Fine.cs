using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;


public partial class Fine
{
    [JsonPropertyName("valor_multa_documento_cobranca_pix")]
    public string? ValueFineDocumentBillingPix { get; set; }
}
