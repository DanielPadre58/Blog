namespace Blog.Application.Dtos.Users;

public record UserDto(
    int Id, 
    string Username, 
    string? FirstName, 
    string? LastName, 
    DateTime? Birthday)
{
    public UserDto(Domain.Entities.User user) : this(
        user.Id,
        user.Username,
        user.FirstName,
        user.LastName,
        user.Birthday
    ){}
}