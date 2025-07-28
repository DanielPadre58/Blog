using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Tags;

public interface ITagRepo
{
    public Task SaveAsync();
    public Task<Tag> CreateAsync(Tag tag);
    public Task<bool> ExistsAsync(string name);
    public Task<Tag?> GetByNameAsync(string name);
}