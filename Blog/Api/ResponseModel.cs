using System.Net;

namespace Blog.Api;

public class ResponseModel<T>
{
    public HttpStatusCode Status { get; set; }
    public string? Message { get; set; }
    public ICollection<T> Data { get; set; } = new List<T>();

    public void Ok(string message, ICollection<T> data)
    {
        Status = HttpStatusCode.OK;
        Message = message;
        Data = data;
    }
    
    public void Ok(string message, T data)
    {
        Status = HttpStatusCode.OK;
        Message = message;
        Data.Add(data);
    }
    
    public void Ok(string message)
    {
        Status = HttpStatusCode.OK;
        Message = message;
    }
    
    public void Created(string message, T data)
    {
        Status = HttpStatusCode.Created;
        Message = message;
        Data.Add(data);
    }
    
    public void BadRequest(string message)
    {
        Status = HttpStatusCode.BadRequest;
        Message = message;
    }
    
    public void NotFound(string message)
    {
        Status = HttpStatusCode.NotFound;
        Message = message;
    }
    
    public void Conflict(string message)
    {
        Status = HttpStatusCode.Conflict;
        Message = message;
    }
    
    public void InternalServerError(string message)
    {
        Status = HttpStatusCode.InternalServerError;
        Message = message;
    }
}