using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class FineModel
{
    [Column("multa_valor_multa_documento_cobranca_pix")]
    public decimal? ValueFineDocumentBillingPix { get; set; }
}
