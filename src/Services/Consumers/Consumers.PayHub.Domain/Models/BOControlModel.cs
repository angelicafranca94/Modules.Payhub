using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consumers.PayHub.Domain.Models;

[Table("BOControle")]
public class BOControlModel
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }


    [Column("TabelaSistemaOrigem")]
    public string OriginSystemTable { get; set; }

    [Column("CodigoIdentificadorSistemaOrigem")]
    public int OriginSystemId { get; set; }

    [Column("NossoNumero")]
    public string OurNumber { get; set; }

}