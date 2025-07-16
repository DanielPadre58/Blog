namespace Blog.Application.Dtos.User;

public record UserUpdateDto(string? Username, string? Firstname, string? Lastname, DateTime? Birthday);