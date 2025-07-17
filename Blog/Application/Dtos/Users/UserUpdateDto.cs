namespace Blog.Application.Dtos.User;

public record UserUpdateDto(string? Username, string? FirstName, string? LastName, DateTime? Birthday)
{
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new ArgumentException("Username cannot be null or empty.", nameof(Username));
        if(FirstName != null && string.IsNullOrWhiteSpace(FirstName))
            throw new ArgumentException("First name cannot be empty.", nameof(FirstName));
        if(LastName != null && string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(LastName));
        if (Birthday > DateTime.Now)
            throw new ArgumentException("Birthday cannot be in the future.", nameof(Birthday));
    }
}