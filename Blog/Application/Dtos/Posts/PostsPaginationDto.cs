
namespace Blog.Application.Dtos.Posts;

public record PostsPaginationDto(int Page, int PageSize, string? AuthorUsername, string? Title, List<string>? Tags);