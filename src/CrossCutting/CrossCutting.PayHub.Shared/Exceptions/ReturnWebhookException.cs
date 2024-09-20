namespace CrossCutting.PayHub.Shared.Exceptions;
public class ReturnWebhookException : Exception
{
    public ReturnWebhookException(string message) : base(message)
    {
    }
}
