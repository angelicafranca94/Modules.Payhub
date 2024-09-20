using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

[Table("WebhookItauBolecodePixDevolucoes")]
public class DevolutionModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("CodigoWebhookItauBolecodePix")]
    public int WebhookItauBolecodePixCode { get; set; }

    [Column("id")]
    public string Id { get; set; }

    [Column("rtrId")]
    public string RtrId { get; set; }

    [Column("valor")]
    public decimal? Amount { get; set; }

    [Column("natureza")]
    public string? Nature { get; set; }

    [Column("descricao")]
    public string? Description { get; set; }

    public virtual TimeModel? Time { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("motivo")]
    public string? Reason { get; set; }

    public virtual WebhookItauBolecodePixModel Pix { get; set; }
}
