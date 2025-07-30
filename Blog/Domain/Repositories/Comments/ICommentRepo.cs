using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Comments;

public interface ICommentRepo
{
    public Task CreateAsync(Comment comment);
    public Task<List<Comment>> GetByPostAsync(int postId);
    public Task<List<Comment>> GetByParentAsync(int parentId);
    public Task<Comment> GetByIdAsync(int id);
}