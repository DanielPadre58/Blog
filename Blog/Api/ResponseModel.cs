using System.Net;

namespace Blog.Api;

public class ResponseModel<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public ICollection<T> Data { get; set; } = new List<T>();
    
    public ResponseModel<T> SuccessResponse(string message)
    {
        Success = true;
        Message = message;
        return this;
    }
    
    public ResponseModel<T> SuccessResponse(string message, T data)
    {
        Success = true;
        Message = message;
        Data.Add(data);
        return this;
    }
    
    public ResponseModel<T> SuccessResponse(string message, ICollection<T> data)
    {
        Success = true;
        Message = message;
        Data = data;
        return this;
    }
    
    public ResponseModel<T> ErrorResponse(string message)
    {
        Success = false;
        Message = message;
        return this;
    }
}