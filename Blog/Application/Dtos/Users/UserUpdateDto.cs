using Blog.Shared.Exceptions;

namespace Blog.Application.Dtos.Users;

public record UserUpdateDto(string? FirstName, string? LastName, DateTime? Birthday)
{
    public void Validate()
    {
        if(FirstName != null && string.IsNullOrWhiteSpace(FirstName))
            throw new InvalidFieldsException("First name cannot be empty.", nameof(FirstName));
        if(LastName != null && string.IsNullOrWhiteSpace(LastName))
            throw new InvalidFieldsException("Last name cannot be empty.", nameof(LastName));
        if (Birthday > DateTime.Now)
            throw new InvalidFieldsException("Birthday cannot be in the future.", nameof(Birthday));
    }
}