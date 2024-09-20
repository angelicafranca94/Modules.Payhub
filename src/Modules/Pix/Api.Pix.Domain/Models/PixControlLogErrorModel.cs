using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Pix.Domain.Models;

[Table("PixControleLogError")]
public class PixControlLogErrorModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("MensagemErro")]
    public string ErrorMessage { get; set; }

    [Column("StackTrace")]
    public string? StackTrace { get; set; }

    [Column("CodigoDebito")]
    public int DebtCode { get; set; }

    [Column("JsonRetorno")]
    public string JsonInput { get; set; }

    [Column("JsonSaida")]
    public string JsonOutput { get; set; }

    [Column("IdTransacao")]
    public string TransactionId { get; set; }
}
