using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Application.Dtos.Posts;

public record PostDto(
    int Id,
    string Title,
    string Content,
    string ImageUrl,
    int Likes,
    int Dislikes,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    UserDto author,
    Tag[] tags
);