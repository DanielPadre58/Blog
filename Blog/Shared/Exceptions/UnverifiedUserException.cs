namespace Blog.Shared.Exceptions;

public class UnverifiedUserException : Exception
{
    public UnverifiedUserException() : base("User is not verified, therefore cannot perform this action.")
    {
    }
    
    public UnverifiedUserException(string message) : base(message)
    {
    }
}