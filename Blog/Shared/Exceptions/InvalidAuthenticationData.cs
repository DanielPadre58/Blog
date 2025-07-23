namespace Blog.Shared.Exceptions;

public class InvalidAuthenticationData : Exception
{
    public InvalidAuthenticationData() : base("Username or password is invalid.")
    {
    }
    
    public InvalidAuthenticationData(string message) : base(message)
    {
    }
}