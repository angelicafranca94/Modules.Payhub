using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class OriginalModel
{
    [Column("original_valor")]
    public decimal? Amount { get; set; }
}
