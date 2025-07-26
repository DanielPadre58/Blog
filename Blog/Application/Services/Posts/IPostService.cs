using Blog.Application.Dtos.Posts;

namespace Blog.Application.Services.Posts;

public interface IPostService
{
    public Task<PostDto> CreateAsync(PostCreationDto dto, string authorUsername);
    public Task<PostDto> GetByIdAsync(int id);
}