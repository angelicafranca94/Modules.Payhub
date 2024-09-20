using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class TimeModel
{
    [Column("horario_solicitacao")]
    public string Solicitation { get; set; }

    [Column("horario_liquidacao")]
    public string Liquidation { get; set; }
}