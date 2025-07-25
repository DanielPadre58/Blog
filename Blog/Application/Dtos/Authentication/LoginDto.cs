using Blog.Shared.Exceptions;

namespace Blog.Application.Dtos.Authentication;

public record LoginDto(string Username, string Password)
{
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new InvalidFieldsException("Username cannot be null or empty.", nameof(Username));
        
        if (string.IsNullOrWhiteSpace(Password))
            throw new InvalidFieldsException("Password cannot be null or empty.", nameof(Password));
    }
}