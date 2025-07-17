namespace Blog.Shared.Exceptions;

public class InvalidFieldsException : Exception
{
    public InvalidFieldsException() : base("Some fiels have been given invalid values"){ }

    public InvalidFieldsException(string message) : base(message) { }
    
    public InvalidFieldsException(string message, string propertyName) 
        : base($"Invalid value for property '{propertyName}': {message}") { }
}