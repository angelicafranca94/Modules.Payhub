using System.Net;

namespace Api.Pix.Infrastructure.Exceptions;
public class RequestFailedException : Exception
{
    public HttpStatusCode StatusCode;

    public RequestFailedException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
