using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consumers.PayHub.Domain.Models;

[Table("BMRemessa")]
public class BMRemessaPixModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Codigo")]
    public int Code { get; set; }

    [Column("NossoNumero_Detalhe")]
    public string? OurNumberDetail { get; set; }

    [Column("CodigoFNDebitos")]
    public int? FNDebitCode { get; set; }

    [Column("Chave")]
    public string Key { get; set; }

    [Column("ChavePix_BoleCode")]
    public string? PixKey_BoleCode { get; set; }

    [Column("IdLocation_BoleCode")]
    public string? LocationId_BoleCode { get; set; }

    [Column("CodigoTipoCobrancaQRCode_BoleCode")]
    public string? QRCodeBillingTypeCode_BoleCode { get; set; }

    [Column("Brancos_BoleCode")]
    public string? Whites_BoleCode { get; set; }

    [Column("ChaveCopiaColaPIX_BoleCode")]
    public string? PixCopyPasteKey_BoleCode { get; set; }

    [Column("TXID")]
    public string? TXID { get; set; }


}