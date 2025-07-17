namespace Blog.Shared.Exceptions;

public class DuplicatedUsernameException : Exception
{
    public DuplicatedUsernameException() : base("A user with this username already exists") { }
    
    public DuplicatedUsernameException(string username) : base($"The username '{username}' is already taken. Please choose a different username.") { }
}