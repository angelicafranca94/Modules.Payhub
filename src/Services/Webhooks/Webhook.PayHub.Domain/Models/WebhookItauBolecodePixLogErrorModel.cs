using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models;

[Table("WebhookItauBolecodePixLogError")]
public class WebhookItauBolecodePixLogErrorModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("MensagemErro")]
    public string ErrorMessage { get; set; }

    [Column("StackTrace")]
    public string? StackTrace { get; set; }

    [Column("JsonRetorno")]
    public string JsonInput { get; set; }

    [Column("IdTransacao")]
    public string TransactionId { get; set; }
}
