using Blog.DbContext;
using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Posts;

public class PostRepo(BlogContext context) : IPostRepo
{
    public Task<Post> GetByIdAsync(int id)
    {
        return context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id) ??
               throw new NotFoundException($"Post with ID {id} not found.");
    }
}