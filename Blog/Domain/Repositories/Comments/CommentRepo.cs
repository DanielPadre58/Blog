using Blog.Domain.DbContext;
using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Comments;

public class CommentRepo(BlogContext context) : ICommentRepo
{
    public async Task CreateAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Comment comment)
    {
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetByPostAsync(int postId)
    {
        return await context.Comments
            .Include(c => c.Author)
            .OrderBy(c => c.Likes)
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetByParentAsync(int parentId)
    {
        return await context.Comments
            .Include(c => c.Author)
            .OrderBy(c => c.Likes)
            .Where(c => c.ParentId == parentId)
            .ToListAsync();
    }

    public async Task<Comment> GetByIdAsync(int id)
    {
        var metadata = await context.Comments
            .Where(c => c.Id == id)
            .Select(c => new { c.Id, c.isReply })
            .FirstOrDefaultAsync();

        if (metadata == null)
            throw new NotFoundException("Comment not found");

        IQueryable<Comment> query = context.Comments
            .Include(c => c.Author);

        query = metadata.isReply
            ? query.Include(c => c.Parent).Include(c => c.Parent.Author)
            : query.Include(c => c.Post).Include(c => c.Post.Author);

        return await query.FirstAsync(c => c.Id == id);
    }
}