namespace Blog.Application.Dtos.User;

public record UserUpdateDto(string? Username, string? FirstName, string? LastName, DateTime? Birthday);