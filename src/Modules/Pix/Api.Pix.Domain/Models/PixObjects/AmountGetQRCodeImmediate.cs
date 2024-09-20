using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.PixObjects;
public class AmountGetQRCodeImmediate
{
    [JsonPropertyName("original")]
    public string Original { get; set; }

    [JsonPropertyName("modalidadeAlteracao")]
    public string ModalidadeAlteracao { get; set; }
}
