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
        return await context.Comments
                   .Include(c => c.Author)
                   .FirstOrDefaultAsync(c => c.Id == id) ??
               throw new NotFoundException("Comment not found");
    }
}