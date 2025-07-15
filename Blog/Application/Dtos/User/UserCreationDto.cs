namespace Blog.Application.Dtos.User;

public record UserCreationDto(
    string Username,
    string Email,
    string Password);