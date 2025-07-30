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
    UserDto author
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
        new UserDto(comment.Author)
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
        new UserDto(comment.Author)
    )
    {
    }
}