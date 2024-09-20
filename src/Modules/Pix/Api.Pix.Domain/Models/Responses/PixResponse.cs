using Api.Pix.Domain.Models.PixObjects;
using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.Responses;
public class PixResponse
{
    [JsonPropertyName("calendario")]
    public Calendary Calendary { get; set; }

    [JsonPropertyName("txid")]
    public string Txid { get; set; }

    [JsonPropertyName("revisao")]
    public int Revision { get; set; }

    [JsonPropertyName("loc")]
    public Loc Loc { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("devedor")]
    public Debtor Debtor { get; set; }

    [JsonPropertyName("valor")]
    public AmountQRCodeImmediate Amount { get; set; }

    [JsonPropertyName("modalidadeAlteracao")]
    public string ChangeModality { get; set; }

    [JsonPropertyName("chave")]
    public string Key { get; set; }

    [JsonPropertyName("pixCopiaECola")]
    public string PixCopyAndPaste { get; set; }

    [JsonPropertyName("solicitacaoPagador")]
    public string PayerRequest { get; set; }

    [JsonPropertyName("infoAdicionais")]
    public IEnumerable<InformationAdditional> AdditionalInfo { get; set; }
}
