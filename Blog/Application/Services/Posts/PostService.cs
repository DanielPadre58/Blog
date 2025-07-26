using Blog.Application.Dtos.Posts;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Posts;
using Blog.Shared.Exceptions;

namespace Blog.Application.Services.Posts;

public class PostService(IPostRepo repository, IUserService userService) : IPostService
{
    public async Task<PostDto> CreateAsync(PostCreationDto dto, string authorUsername)
    {
        if(dto == null)
            throw new InvalidFieldsException("Post creation data cannot be null");
        
        var author = await userService.GetByUsernameAsync(authorUsername);
        
        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content ?? string.Empty,
            ImageUrl = dto.ImageUrl ?? string.Empty,
            Tags = dto.Tags ?? new List<Tag>(),
            AuthorId = author.Id,
            CreatedAt = DateTime.UtcNow,
        };
        
        post.Validate();

        repository.CreateAsync(post);
        
        return new PostDto(post);
    }

    public async Task<PostDto> GetByIdAsync(int id)
    {
        if(id <= 0)
            throw new InvalidFieldsException("Invalid post ID");
        
        var post = await repository.GetByIdAsync(id);
        
        return new PostDto(post);
    }
}