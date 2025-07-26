using Blog.Application.Dtos.Posts;
using Blog.Domain.Repositories.Posts;
using Blog.Shared.Exceptions;

namespace Blog.Application.Services.Posts;

public class PostService(IPostRepo repository) : IPostService
{
    public async Task<PostDto> GetByIdAsync(int id)
    {
        if(id <= 0)
            throw new InvalidFieldsException("Invalid post ID");
        
        var post = await repository.GetByIdAsync(id);
        
        return new PostDto(post);
    }
}