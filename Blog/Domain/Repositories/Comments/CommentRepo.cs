using Blog.Domain.DbContext;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Comments;

public class CommentRepo(BlogContext context) : ICommentRepo
{
    public async Task CreateAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
    }
}