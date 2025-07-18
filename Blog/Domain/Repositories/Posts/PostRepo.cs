using Blog.DbContext;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Posts;

public class PostRepo(BlogContext context) : IPostRepo
{
    public async Task<Post> Create(Post post)
    {
        await context.Posts.AddAsync(post);

        return post;
    }
}