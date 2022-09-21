using System.Net;
namespace Domain.Wrapper;

public class Responce<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public Responce(T data)
    {
        Data = data;
        StatusCode = HttpStatusCode.OK;
        Message = null;
    }

    public Responce(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
