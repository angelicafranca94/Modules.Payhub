using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consumers.PayHub.Domain.Models;

[Table("WebhookItauBolecodePix")]
public class WebhookItauBolecodePixModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("EndToEndId")]
    public string EndToEndId { get; set; }

    [Column("Txid")]
    public string? Txid { get; set; }

    [Column("Valor")]
    public decimal Amount { get; set; }

    [Column("Horario")]
    public DateTime Timestamp { get; set; }

    [Column("InfoPagador")]
    public string? InfoPay { get; set; }

    [Column("Chave")]
    public string? Key { get; set; }

    [Column("FilaRabbitMQProcessada")]
    public bool RabbitMQQueueProcessed { get; set; }

    [Column("DataHoraFilaRabbitMQProcessada")]
    public DateTime? DatetimeRabbitMQQueueProcessed { get; set; }
}
