using Blog.Application.Dtos.Posts;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Posts;

public interface IPostRepo
{
    public Task SaveAsync();
    public Task CreateAsync(Post post);
    public Task DeleteAsync(Post post);
    public Task<Post> GetByIdAsync(int id);
    public Task<List<Post>> GetAllAsync(int page, int pageSize);
    public Task<List<Post>> GetByTitleAsync(int page, int pageSize, string title);
    public Task<List<Post>> GetByLikesAsync(int page, int pageSize);
    public Task<List<Post>> GetByTagsAsync(int page, int pageSize, List<string> tags);
    public Task<List<Post>> GetByAuthorAsync(int page, int pageSize, string authorUsername);
    public Task<List<Post>> GetUserLikesAsync(int page, int pageSize, string username);
    public Task<List<Post>> GetUserDislikesAsync(int page, int pageSize, string username);
    public Task<List<Post>> GetUserCommentedPostsAsync(int page, int pageSize, string username);
}