using Blog.Application.Dtos.Comments;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Comments;
using Blog.Shared.Exceptions;

namespace Blog.Application.Services.Comments;

public class CommentService(
    ICommentRepo repository,
    IUserService userService) : ICommentService
{
    public async Task<CommentDto> CreateAsync(CommentCreationDto dto, string authorUsername)
    {
        if (dto == null)
            throw new InvalidFieldsException("Comment creation data cannot be null.");

        if (dto.parentId == null && dto.postId == null)
            throw new InvalidFieldsException("A comment must have either a Post ID or a Parent Comment ID.");

        if (dto.parentId != null && dto.postId != null)
            throw new InvalidFieldsException("A comment cannot be both a reply and a top-level comment.");
        
        var author = await userService.GetByUsernameAsync(authorUsername);
        
        var comment = new Comment
        {
            Content = dto.Content,
            PostId = dto.postId,
            ParentId = dto.parentId,
            AuthorId = author.Id
        };

        repository.CreateAsync(comment);
        
        return new CommentDto(comment);
    }
}