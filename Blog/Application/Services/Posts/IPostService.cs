using Blog.Application.Dtos.Posts;
using Blog.Domain.Enums;

namespace Blog.Application.Services.Posts;

public interface IPostService
{
    public Task<PostDto> CreateAsync(PostCreationDto dto, string authorUsername);
    public Task DeleteAsync(int postId, string username);
    public Task<PostDto> GetByIdAsync(int id);
    public Task<List<PostDto>> GetAllAsync(PostsPaginationDto pageInfo, PostFilter filter, string username);
    public Task<PostDto> LikePostAsync(int id, string username);
    public Task<PostDto> DislikePostAsync(int id, string username);
    public Task<List<PostDto>> GetByUsernameAsync(string username, PageInfo pageInfo, UserPostsFilter filter);
}