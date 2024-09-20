using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

[Table("WebhookItauBolecodePixComponentesValor")]
public partial class ComponentsAmountModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("CodigoWebhookItauBolecodePix")]
    public int WebhookItauBolecodePixCode { get; set; }

    public virtual OriginalModel? Original { get; set; }

    public virtual LootModel? Loot { get; set; }

    public virtual ChangeModel? Change { get; set; }

    public virtual FeesModel? Fees { get; set; }

    public virtual FineModel? Fine { get; set; }

    public virtual ReductionModel? Reduction { get; set; }

    public virtual DiscountModel? Discount { get; set; }

    public virtual WebhookItauBolecodePixModel Pix { get; set; }
}
