using Blog.Application.Dtos.Posts;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Posts;

public interface IPostRepo
{
    public Task CreateAsync(Post post);
    public Task<Post> GetByIdAsync(int id);
}