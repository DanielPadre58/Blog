namespace Blog.Application.Dtos.Posts;

public record PostCreationDto(
    string Title,
    string? Content,
    string? ImageUrl,
    List<string>? Tags
);