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
    bool likedByUser,
    DateTime CreatedAt,
    UserDto author,
    List<string> tags
    )
{ 
    public PostDto(Post post, bool likedPost) : this(
        post.Id,
        post.Title,
        post.Content,
        post.ImageUrl,
        post.Likes,
        post.Dislikes,
        likedPost,
        post.CreatedAt,
        new UserDto(post.Author),
        post.Tags.Select(tag => tag.Name).ToList()
    ) { }
}