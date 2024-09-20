using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Pix.Domain.Models;

[Table("PixControle")]
public class PixControlModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("SistemaOrigem")]
    public string SourceSystem { get; set; }

    [Column("TabelaSistemaOrigem")]
    public string SourceSystemTable { get; set; }

    [Column("CodigoIdentificadorSistemaOrigem")]
    public int OriginSystemIdentifierCode { get; set; }

    [Column("ChaveDict")]
    public string DictKey { get; set; }

    [Column("Valor")]
    public decimal Amount { get; set; }

    [Column("IdTransacao")]
    public string TransactionId { get; set; }

    [Column("DataTransacao")]
    public DateTime DateTransaction { get; set; }

    [Column("DataHoraCadastro")]
    public DateTime DateTimeRegistration { get; set; }

    [Column("DataHoraAtualizado")]
    public DateTime? DateTimeUpdated { get; set; }

    [Column("PixCopiaCola")]
    public string PixCopyAndPaste { get; set; }

    [Column("TempoExpiracao")]
    public int ExpirationTime { get; set; }

    [Column("JsonRetorno")]
    public string JsonInput { get; set; }

    [Column("JsonSaida")]
    public string JsonOutput { get; set; }

    [Column("CodigoPixStatusProcessamento")]
    public int CodePixStatusProcessing { get; set; }

}
