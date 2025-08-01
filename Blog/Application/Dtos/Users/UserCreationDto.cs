namespace Blog.Application.Dtos.Users;

public record UserCreationDto(
    string Username,
    string Email,
    string Password);