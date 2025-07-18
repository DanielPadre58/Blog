namespace Blog.Domain.Repositories.Posts;

public interface IPostRepo
{
    public Task<Post> Create(Post post);
}