using Blog.Domain.Entities;

namespace Blog.Application.Dtos.Posts;

public record PostCreationDto(
    string Title,
    string? Content,
    string? ImageUrl,
    List<Tag>? Tags
);