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
)
{
    public PostDto(Post post) : this(
        post.Id,
        post.Title,
        post.Content,
        post.ImageUrl,
        post.Likes,
        post.Dislikes,
        post.CreatedAt,
        post.UpdatedAt,
        new UserDto(post.Author),
        post.Tags.ToArray()
    ) { }
}