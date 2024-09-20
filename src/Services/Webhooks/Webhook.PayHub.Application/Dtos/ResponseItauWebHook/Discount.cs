using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;


public partial class Discount
{
    [JsonPropertyName("valor_desconto_documento_cobranca_pix")]
    public string? ValueDiscountDocumentBillingPix { get; set; }
}
