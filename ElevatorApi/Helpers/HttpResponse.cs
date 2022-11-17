namespace ElevatorApi.Helpers;

public class HttpResponse<T>
{
    public HttpResponse(T data)
    {
        Data = data;
    }
    public T Data { get; }
}