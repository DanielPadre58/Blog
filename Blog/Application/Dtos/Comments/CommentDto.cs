using Blog.Application.Dtos.Posts;
using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Application.Dtos.Comments;

public record CommentDto(
    int id,
    string content,
    int likes,
    int dislikes,
    bool likedByUser,
    bool dislikedByUser,
    DateTime commentDate,
    UserDto author,
    PostDto post,
    CommentDto? Parent
)
{
    public CommentDto(Comment comment) : this(
        comment.Id,
        comment.Content,
        comment.Likes,
        comment.Dislikes,
        false,
        false,
        comment.CommentDate,
        new UserDto(comment.Author),
        comment.Post is not null ? new PostDto(comment.Post) : null,
        comment.Parent is not null ? new CommentDto(comment.Parent) : null
    )
    {
    }
    
    public CommentDto(Comment comment, bool liked, bool disliked) : this(
        comment.Id,
        comment.Content,
        comment.Likes,
        comment.Dislikes,
        liked,
        disliked,
        comment.CommentDate,
        new UserDto(comment.Author),
        comment.Post is not null ? new PostDto(comment.Post) : null,
        comment.Parent is not null ? new CommentDto(comment.Parent) : null
    )
    {
    }
}