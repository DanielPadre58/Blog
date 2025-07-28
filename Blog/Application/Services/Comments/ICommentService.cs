using Blog.Application.Dtos.Comments;

namespace Blog.Application.Services.Comments;

public interface ICommentService
{
    public Task<CommentDto> CreateAsync(CommentCreationDto dto, string authorUsername);
}