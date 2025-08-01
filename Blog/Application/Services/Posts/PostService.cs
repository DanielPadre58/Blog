using Blog.Application.Dtos.Posts;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Repositories.Posts;
using Blog.Domain.Repositories.Tags;
using Blog.Shared.Exceptions;
using Blog.Shared.Validation;

namespace Blog.Application.Services.Posts;

public class PostService(
    IPostRepo repository,
    ITagRepo tagRepo,
    IUserService userService,
    IValidator validator) : IPostService
{
    public async Task<PostDto> CreateAsync(PostCreationDto dto, string authorUsername)
    {
        validator.NotNull(dto, "Post information");

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

        return new PostDto(post);
    }

    public async Task DeleteAsync(int postId, string username)
    {
        validator.ValidId(postId, "Post id");
        validator.NotNullOrEmpty(username, "Username");
        
        var post = await repository.GetByIdAsync(postId);

        if (post.Author.Username == username)
            await repository.DeleteAsync(post);
        else
            throw new UnauthorizedAccessException("Only the author of the post can access this feature");
    }

    public async Task<PostDto> GetByIdAsync(int id)
    {
        validator.ValidId(id, "Post Id");

        var post = await repository.GetByIdAsync(id);

        return new PostDto(post);
    }

    public async Task<List<PostDto>> GetAllAsync(PostsPaginationDto pageInfo, PostFilter filter, string username)
    {
        validator.NotNull(pageInfo, "Page info");

        List<Post> posts;

        switch (filter)
        {
            case PostFilter.AUTHOR:
                posts = await repository.GetByAuthorAsync(
                    pageInfo.page.pageNumber, 
                    pageInfo.page.pageSize, 
                    pageInfo.AuthorUsername ?? string.Empty);
                break;
            case PostFilter.TAGS:
                var tags = pageInfo.Tags ?? new List<string>();
                var existingTags = new List<string>();
                foreach (var tag in tags.Distinct())
                    if (await tagRepo.ExistsAsync(tag))
                        existingTags.Add(tag);
                
                posts = await repository.GetByTagsAsync(
                    pageInfo.page.pageNumber, 
                    pageInfo.page.pageSize, 
                    existingTags);
                break;
            case PostFilter.LIKES:
                posts = await repository.GetByLikesAsync(
                    pageInfo.page.pageNumber, 
                    pageInfo.page.pageSize);
                break;
            case PostFilter.TITLE:
                posts = await repository.GetByTitleAsync(
                    pageInfo.page.pageNumber, 
                    pageInfo.page.pageSize, 
                    pageInfo.Title ?? string.Empty);
                break;
            case PostFilter.NONE:
                posts = await repository.GetAllAsync(
                    pageInfo.page.pageNumber, 
                    pageInfo.page.pageSize);
                break;
            default:
                throw new InvalidFieldsException("Invalid post filter");
        }

        var postDtos = new List<PostDto>();

        foreach (var post in posts)
        {
            var liked = await userService.UserLikedAsync(post, username);
            var disliked = await userService.UserDislikedAsync(post, username);
            postDtos.Add(new PostDto(post, liked, disliked));
        }

        return postDtos;
    }

    public async Task<PostDto> LikePostAsync(int id, string username)
    {
        validator.ValidId(id, "Post Id");
        validator.NotNullOrEmpty(username, "Username");

        var post = await repository.GetByIdAsync(id);
        var liked = await userService.LikeAsync(post, username);

        return new PostDto(post, liked, false);
    }

    public async Task<PostDto> DislikePostAsync(int id, string username)
    {
        validator.ValidId(id, "Post Id");
        validator.NotNullOrEmpty(username, "Username");

        var post = await repository.GetByIdAsync(id);
        var disliked = await userService.DislikeAsync(post, username);

        return new PostDto(post, false, disliked);
    }

    public async Task<List<PostDto>> GetByUsernameAsync(string username, PageInfo pageInfo, UserPostsFilter filter)
    {
        validator.NotNullOrEmpty(username, "Username");
        validator.NotNull(pageInfo, "Pagination information");

        List<Post> posts;
        
        switch (filter)
        {
            case UserPostsFilter.MINE:
                posts = await repository.GetByAuthorAsync(
                    pageInfo.pageNumber,
                    pageInfo.pageSize,
                    username);
                break;
            case UserPostsFilter.LIKED:
                posts = await repository.GetUserLikesAsync(
                    pageInfo.pageNumber, 
                    pageInfo.pageSize,
                    username);
                break;
            case UserPostsFilter.DISLIKED:
                posts = await repository.GetUserDislikesAsync(
                    pageInfo.pageNumber, 
                    pageInfo.pageSize, 
                    username);
                break;
            case UserPostsFilter.COMMENTED:
                posts = await repository.GetUserCommentedPostsAsync(
                    pageInfo.pageNumber, 
                    pageInfo.pageSize, 
                    username);
                break;
            default:
                throw new InvalidFieldsException("Invalid post filter");
        }
        
        var postDtos = new List<PostDto>();

        foreach (var post in posts)
        {
            var liked = await userService.UserLikedAsync(post, username);
            var disliked = await userService.UserDislikedAsync(post, username);
            postDtos.Add(new PostDto(post, liked, disliked));
        }

        return postDtos;
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