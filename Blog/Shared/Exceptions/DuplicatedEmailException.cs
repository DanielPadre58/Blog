namespace Blog.Shared.Exceptions;

public class DuplicatedEmailException : Exception
{
    public DuplicatedEmailException() : base("An account with this email already exists") { }
    
    public DuplicatedEmailException(string email) : base($"The email '{email}' is already taken. Please choose a different email.") { }
}