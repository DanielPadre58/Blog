using System.Net;

namespace Blog.Api;

public class ResponseModel<T>
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
    public ICollection<T> Data { get; set; } = new List<T>();
}