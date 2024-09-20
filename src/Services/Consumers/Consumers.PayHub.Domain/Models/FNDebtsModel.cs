using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consumers.PayHub.Domain.Models;


[Table("FNDebitos")]
public class FNDebtsModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("ValorDebito")]
    public decimal DebtAmount { get; set; }

    [Column("DataPagamento")]
    public DateTime? PaymentDate { get; set; }

    [Column("ValorPago")]
    public decimal? PaymentAmount { get; set; }

    [Column("CodigoWebhookItauBolecodePix")]
    public int? WebhookItauBolecodePixCode { get; set; }
}
