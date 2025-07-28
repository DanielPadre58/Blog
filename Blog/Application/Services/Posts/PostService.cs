using Blog.Application.Dtos.Posts;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Repositories.Posts;
using Blog.Domain.Repositories.Tags;
using Blog.Shared.Exceptions;

namespace Blog.Application.Services.Posts;

public class PostService(
    IPostRepo repository,
    ITagRepo tagRepo,
    IUserService userService) : IPostService
{
    public async Task<PostDto> CreateAsync(PostCreationDto dto, string authorUsername)
    {
        if (dto == null)
            throw new InvalidFieldsException("Post creation data cannot be null");

        var author = await userService.GetByUsernameAsync(authorUsername);

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content ?? string.Empty,
            ImageUrl = dto.ImageUrl ?? string.Empty,
            Tags = await VerifyTagsAsync(dto.Tags),
            AuthorId = author.Id,
            CreatedAt = DateTime.UtcNow,
        };

        post.Validate();

        await repository.CreateAsync(post);

        return new PostDto(post, false);
    }

    public async Task<PostDto> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new InvalidFieldsException("Invalid post ID");

        var post = await repository.GetByIdAsync(id);

        return new PostDto(post, false);
    }

    public async Task<List<PostDto>> GetAllAsync(PostsPaginationDto pageInfo, PostFilter filter, string username)
    {
        if (pageInfo == null)
            throw new InvalidFieldsException("Please provide pagination information");

        List<Post> posts;

        switch (filter)
        {
            case PostFilter.AUTHOR:
                posts = await repository.GetByAuthorAsync(pageInfo.Page, pageInfo.PageSize, pageInfo.AuthorUsername ?? string.Empty);
                break;
            case PostFilter.TAGS:
                var tags = pageInfo.Tags ?? new List<string>();
                var existingTags = new List<string>();
                foreach (var tag in tags.Distinct())
                    if (await tagRepo.ExistsAsync(tag))
                        existingTags.Add(tag);
                posts = await repository.GetByTagsAsync(pageInfo.Page, pageInfo.PageSize, existingTags);
                break;
            case PostFilter.LIKES:
                posts = await repository.GetByLikesAsync(pageInfo.Page, pageInfo.PageSize);
                break;
            case PostFilter.TITLE:
                posts = await repository.GetByTitleAsync(pageInfo.Page, pageInfo.PageSize, pageInfo.Title ?? string.Empty);
                break;
            case PostFilter.NONE:
                posts = await repository.GetAllAsync(pageInfo.Page, pageInfo.PageSize);
                break;
            default:
                throw new InvalidFieldsException("Invalid post filter");
        }

        var postDtos = new List<PostDto>();

        foreach (var post in posts)
        {
            var liked = await userService.UserLikedAsync(post, username);
            postDtos.Add(new PostDto(post, liked));
        }

        return postDtos;
    }

    public async Task<PostDto> LikePostAsync(int id, string username)
    {
        if (id <= 0)
            throw new InvalidFieldsException("Invalid post ID");

        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");

        var post = await repository.GetByIdAsync(id);
        post.Like();
        await userService.LikePostAsync(post, username);
        await repository.SaveAsync();

        return new PostDto(post, true);
    }

    private async Task<List<Tag>> VerifyTagsAsync(List<string>? tagsNames)
    {
        var result = new List<Tag>();

        if (tagsNames == null)
            return result;

        foreach (var tagName in tagsNames.Distinct())
        {
            var tag = await tagRepo.GetByNameAsync(tagName);

            if (tag == null)
                tag = await tagRepo.CreateAsync(new Tag { Name = tagName });
            else
            {
                tag.Use();
                await tagRepo.SaveAsync();
            }

            result.Add(tag);
        }

        return result;
    }
}