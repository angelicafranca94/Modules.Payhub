using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;
public partial class ReductionModel
{
    [Column("abatimento_valor_abatimento_documento_cobranca_pix")]
    public decimal? ValueRebateDocumentBillingPix { get; set; }
}