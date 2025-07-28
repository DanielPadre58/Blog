
namespace Blog.Application.Dtos.Posts;

public record PostsPaginationDto(PageInfo page, string? AuthorUsername, string? Title, List<string>? Tags);