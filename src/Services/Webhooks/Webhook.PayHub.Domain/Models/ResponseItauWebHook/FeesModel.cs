using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class FeesModel
{
    [Column("juros_valor_juros")]
    public decimal? InterestValue { get; set; }
}