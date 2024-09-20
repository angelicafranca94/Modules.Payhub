using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class ChangeModel
{
    [Column("troco_valor")]
    public decimal Amount { get; set; }

    [Column("troco_modalidadeAgente")]
    public string AgentModality { get; set; }

    [Column("troco_prestadorDeServicoDeSaque")]
    public string WithdrawalServiceProvider { get; set; }
}