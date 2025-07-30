using Blog.Application.Dtos.Comments;

namespace Blog.Application.Services.Comments;

public interface ICommentService
{
    public Task<CommentDto> CreateAsync(CommentCreationDto dto, string authorUsername);
    public Task<List<CommentDto>> GetByPostAsync(int postId, string username);
    public Task<List<CommentDto>> GetByParentAsync(int parentId, string username);
    public Task<CommentDto> LikeCommentAsync(int id, string username);
    public Task<CommentDto> DislikeCommentAsync(int id, string username);
}