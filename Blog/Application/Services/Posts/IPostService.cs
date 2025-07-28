using Blog.Application.Dtos.Posts;
using Blog.Domain.Enums;

namespace Blog.Application.Services.Posts;

public interface IPostService
{
    public Task<PostDto> CreateAsync(PostCreationDto dto, string authorUsername);
    public Task<PostDto> GetByIdAsync(int id);
    public Task<List<PostDto>> GetAllAsync(PostsPaginationDto pageInfo, PostFilter filter, string username);
    public Task<PostDto> LikePostAsync(int id, string username);
}