using Blog.Application.Dtos.Comments;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Comments;

public interface ICommentRepo
{
    public Task CreateAsync(Comment comment);
}