namespace CrossCutting.PayHub.Shared.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
