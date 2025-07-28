namespace Blog.Application.Dtos.Comments;

public record CommentCreationDto(
    string Content,
    int? postId,
    int? parentId);