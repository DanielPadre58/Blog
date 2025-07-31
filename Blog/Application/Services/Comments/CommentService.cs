using Blog.Application.Dtos.Comments;
using Blog.Application.Services.Posts;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Comments;
using Blog.Shared.Exceptions;

namespace Blog.Application.Services.Comments;

public class CommentService(
    ICommentRepo repository,
    IUserService userService,
    IPostService postService) : ICommentService
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
            isReply = dto.parentId is null,
            Content = dto.Content,
            PostId = dto.postId,
            ParentId = dto.parentId,
            AuthorId = author.Id
        };

        await repository.CreateAsync(comment);

        return new CommentDto(comment);
    }

    public async Task DeleteAsync(int commentId, string loggedUsername)
    {
        var comment = await repository.GetByIdAsync(commentId);

        var isCommentAuthor = comment.Author.Username == loggedUsername;
        var isPostAuthor = !comment.isReply && comment.Post?.Author.Username == loggedUsername;

        if (!isCommentAuthor && !isPostAuthor)
            throw new UnauthorizedAccessException("Only the author of the comment or the author of the post can access this feature");

        await repository.DeleteAsync(comment);
    }

    public async Task<List<CommentDto>> GetByPostAsync(int postId, string username)
    {
        if (postId <= 0)
            throw new InvalidFieldsException("Invalid post ID");

        await postService.GetByIdAsync(postId);

        var comments = await repository.GetByPostAsync(postId);

        var commentDtos = new List<CommentDto>();

        foreach (var comment in comments)
        {
            var liked = await userService.UserLikedAsync(comment, username);
            var disliked = await userService.UserDislikedAsync(comment, username);
            commentDtos.Add(new CommentDto(comment, liked, disliked));
        }

        return commentDtos;
    }

    public async Task<List<CommentDto>> GetByParentAsync(int parentId, string username)
    {
        if (parentId <= 0)
            throw new InvalidFieldsException("Invalid post ID");

        await repository.GetByIdAsync(parentId);

        var comments = await repository.GetByParentAsync(parentId);

        var commentDtos = new List<CommentDto>();

        foreach (var comment in comments)
        {
            var liked = await userService.UserLikedAsync(comment, username);
            var disliked = await userService.UserDislikedAsync(comment, username);
            commentDtos.Add(new CommentDto(comment, liked, disliked));
        }

        return commentDtos;
    }

    public async Task<CommentDto> LikeCommentAsync(int id, string username)
    {
        if (id <= 0)
            throw new InvalidFieldsException("Invalid comment ID");

        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");

        var comment = await repository.GetByIdAsync(id);
        var liked = await userService.LikeAsync(comment, username);

        return new CommentDto(comment, liked, false);
    }

    public async Task<CommentDto> DislikeCommentAsync(int id, string username)
    {
        if (id <= 0)
            throw new InvalidFieldsException("Invalid comment ID");

        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");

        var comment = await repository.GetByIdAsync(id);
        var disliked = await userService.DislikeAsync(comment, username);

        return new CommentDto(comment, false, disliked);
    }
}