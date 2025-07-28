using Blog.Domain.DbContext;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Tags;

public class TagRepo(BlogContext context) : ITagRepo
{
    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Tag> CreateAsync(Tag tag)
    {
        tag.Name = tag.Name.ToUpper();
        
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        
        return tag;
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await context.Tags
            .AnyAsync(t => t.Name == name.ToUpper());
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await context.Tags
            .FirstOrDefaultAsync(t => t.Name == name.ToUpper());
    }
}