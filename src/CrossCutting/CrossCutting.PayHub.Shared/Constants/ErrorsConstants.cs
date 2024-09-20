namespace CrossCutting.PayHub.Shared.Constants;

public static class ErrorsConstants
{
    public const string PixTransactionInactive = "Transação não se encontra mais ativa. Favor realize uma nova transação";
    public const string PixTransactionExpired = "Transação expirada Favor realize uma nova transação";
    public const string DebitNotFound = "Débito não encontrado.";
    public const string DecryptNotWork = "Erro ao descriptografar os dados:  ";
    public const string ReturnWebhookError = "Erro de retorno do webhook. Objeto nulo.";
    public const string PixTransactionNotFound = "Transação não encontrada";
    public const string PublishQueueTimeOut = "Tempo de publicação excedeu o tempo limite.";
    public const string WebhookJsonNotFound = "WebhookJson referente ao pagamento não encontrado";
    public const string AccountBankNotFound = "Conta Corrente não encontrada";
    public const string PixKeyNotFound = "Chave pix não encontrada. Não é possivel gerar copia e cola sem a chave pix do recebedor.";
    public const string ItauFilesNotFound = "Não foram encontrados crt ou arquivo de chave privada para esse cnpj {cnpj}";
    public const string ClientIdNotFound = "Client_Id pix não encontrada. Não é possivel gerar copia e cola sem o Client_Id do recebedor.";
    public const string ClientSecretNotFound = "ClientSecret pix não encontrada. Não é possivel gerar o token para requisição sem o ClientSecret do recebedor.";
}
