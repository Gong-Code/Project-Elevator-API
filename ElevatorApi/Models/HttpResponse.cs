namespace ElevatorApi.Models;

public class HttpResponse<T> 
{
    public HttpResponse(T data)
    {
        Data = data;
    }

    public T Data { get; set; }
}