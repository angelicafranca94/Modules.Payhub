using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.PayHub.Domain.Models.ResponseItauWebHook;

public partial class LootModel
{
    [Column("saque_valor")]
    public decimal Amount { get; set; }

    [Column("saque_modalidadeAgente")]
    public string AgentModality { get; set; }

    [Column("saque_prestadorDeServicoDeSaque")]
    public string WithdrawalServiceProvider { get; set; }
}