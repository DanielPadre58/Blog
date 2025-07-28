using Blog.DbContext;
using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Posts;

public class PostRepo(BlogContext context) : IPostRepo
{
    public Task CreateAsync(Post post)
    {
        context.Posts.Add(post);
        return context.SaveChangesAsync();
    }

    public Task<Post> GetByIdAsync(int id)
    {
        return context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id) ??
               throw new NotFoundException($"Post with ID {id} not found.");
    }

    public Task<List<Post>> GetAllAsync(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
            throw new InvalidFieldsException("Page and page size must be greater than zero.");

        return context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Post>> GetByTitleAsync(int page, int pageSize, string title)
    {
        if (page < 1 || pageSize < 1)
            throw new InvalidFieldsException("Page and page size must be greater than zero.");
        
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidFieldsException("Title cannot be null or empty.");

        return await context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .Where(p => p.Title.ToLower().Contains(title.ToLower()))
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Post>> GetByLikesAsync(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
            throw new InvalidFieldsException("Page and page size must be greater than zero.");

        return await context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .OrderByDescending(p => p.Likes)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Post>> GetByTagsAsync(int page, int pageSize, List<string> tags)
    {
        if (page < 1 || pageSize < 1)
            throw new InvalidFieldsException("Page and page size must be greater than zero.");

        if (tags.Count == 0)
            return await GetAllAsync(page, pageSize);

        return await context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .Where(p => tags.All(name => p.Tags.Any(t => t.Name == name)))
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Post>> GetByAuthorAsync(int page, int pageSize, string authorUsername)
    {
        if (page < 1 || pageSize < 1)
            throw new InvalidFieldsException("Page and page size must be greater than zero.");

        if (string.IsNullOrWhiteSpace(authorUsername))
            throw new InvalidFieldsException("Author username cannot be null or empty.");

        return context.Posts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .Where(p => p.Author.Username.ToLower().StartsWith(authorUsername.ToLower()))
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}