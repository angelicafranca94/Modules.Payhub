using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Pix.Domain.Models;

[Table("FNContaCorrente")]
public class AccountsBankModel
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("ChavePix")]
    public string? PixKey { get; set; }

    [Column("CNPJ")]
    public string CNPJ { get; set; }

    [Column("client_id")]
    public string ClientId { get; set; }

    [Column("client_secret")]
    public string ClientSecret { get; set; }
}
