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
    bool dislikedByUser,
    DateTime CreatedAt,
    UserDto author,
    List<string> tags
    )
{ 
    public PostDto(Post post) : this(
        post.Id,
        post.Title,
        post.Content,
        post.ImageUrl,
        post.Likes,
        post.Dislikes,
        false,
        false,
        post.CreatedAt,
        new UserDto(post.Author),
        post.Tags.Select(tag => tag.Name).ToList()
    ) { }
    
    public PostDto(Post post, bool likedPost, bool dislikedPost) : this(
        post.Id,
        post.Title,
        post.Content,
        post.ImageUrl,
        post.Likes,
        post.Dislikes,
        likedPost,
        dislikedPost,
        post.CreatedAt,
        new UserDto(post.Author),
        post.Tags.Select(tag => tag.Name).ToList()
    ) { }
}