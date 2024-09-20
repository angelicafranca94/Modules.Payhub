using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class DiscountModel
{
    [Column("desconto_valor_desconto_documento_cobranca_pix")]
    public decimal? ValueDiscountDocumentBillingPix { get; set; }
}
