using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Pix.Domain.Models;


[Table("FNDebitos")]
public class FNDebtsModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("ValorDebito")]
    public decimal DebtValue { get; set; }

    [Column("DescricaoDebito")]
    public string DebtDescription { get; set; }

    [Column("CodigoContaCorrente")]
    public int AccountBankCode { get; set; }
}
