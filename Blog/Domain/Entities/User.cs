using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Shared.Exceptions;
using Blog.Shared.Validation;
using EmailValidation;

namespace Blog.Domain.Entities;

public class User(IValidator validator)
{
    public int Id { get; set; }
    public bool IsVerified { get; set; }
    [Required] public required string Username { get; set; }
    [Required] public required string Email { get; set; }
    [Required] public required string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore] public ICollection<Post>? Posts { get; set; } = new List<Post>();
    [JsonIgnore] public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    [JsonIgnore] public ICollection<Post>? LikedPosts { get; set; } = new List<Post>();
    [JsonIgnore] public ICollection<Post>? DislikedPosts { get; set; } = new List<Post>();
    [JsonIgnore] public ICollection<Comment>? LikedComments { get; set; } = new List<Comment>();
    [JsonIgnore] public ICollection<Comment>? DislikedComments { get; set; } = new List<Comment>();

    public void ChangeUsername(string? username)
    {
        if (username == null)
            return;

        validator.NotNullOrEmpty(username, nameof(username));
        Username = username;
    }

    public void ChangeFirstName(string? name)
    {
        if (name == null)
            return;

        validator.NotNullOrEmpty(name, nameof(name));
        FirstName = name;
    }

    public void ChangeLastName(string? name)
    {
        if (name == null)
            return;

        validator.NotNullOrEmpty(name, nameof(name));
        LastName = name;
    }

    public void ChangeBirthday(DateTime? birthday)
    {
        if (!birthday.HasValue)
            return;

        validator.BeforeToday(birthday.Value, nameof(Birthday));
        Birthday = birthday;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public void Validate()
    {
        validator.NotNullOrEmpty(Username, nameof(Username));
        validator.ValidEmail(Email, nameof(Email));
        validator.ValidPassword(Password, nameof(Password));
        if (FirstName != null)
            validator.NotNullOrEmpty(FirstName, nameof(FirstName));

        if (LastName != null)
            validator.NotNullOrEmpty(LastName, nameof(LastName));

        if (Birthday.HasValue)
        {
            validator.BeforeToday(Birthday.Value, nameof(Birthday));
            validator.OlderThan(Birthday.Value, 16, nameof(Birthday));
        }
    }

    public void Like<T>(T interactable) where T : class
    {
        if (interactable is Post post)
        {
            if (LikedPosts!.Any(p => p.Id == post.Id))
            {
                LikedPosts!.Remove(LikedPosts.First(p => p.Id == post.Id));
                post.RemoveLike(this);
                return;
            }

            if (Liked<Post>(post.Id))
            {
                DislikedPosts!.Remove(DislikedPosts.First(p => p.Id == post.Id));
                post.RemoveDislike(this);
            }

            LikedPosts!.Add(post);
            post.Like(this);
        }

        if (interactable is Comment comment)
        {
            if (LikedComments!.Any(c => c.Id == comment.Id))
            {
                LikedComments!.Remove(LikedComments.First(c => c.Id == comment.Id));
                comment.RemoveLike(this);
                return;
            }

            if (Disliked<Comment>(comment.Id))
            {
                DislikedComments!.Remove(DislikedComments.First(c => c.Id == comment.Id));
                comment.RemoveDislike(this);
            }

            LikedComments!.Add(comment);
            comment.Like(this);
        }
    }

    public void Dislike<T>(T interactable) where T : class
    {
        if (interactable is Post post)
        {
            if (DislikedPosts!.Any(p => p.Id == post.Id))
            {
                DislikedPosts!.Remove(DislikedPosts.First(p => p.Id == post.Id));
                post.RemoveDislike(this);
                return;
            }

            if (Liked<Post>(post.Id))
            {
                LikedPosts!.Remove(LikedPosts.First(p => p.Id == post.Id));
                post.RemoveLike(this);
            }

            DislikedPosts!.Add(post);
            post.Dislike(this);
        }

        if (interactable is Comment comment)
        {
            if (DislikedComments!.Any(c => c.Id == comment.Id))
            {
                DislikedComments!.Remove(DislikedComments.First(c => c.Id == comment.Id));
                comment.RemoveDislike(this);
                return;
            }

            if (Liked<Comment>(comment.Id))
            {
                LikedComments!.Remove(LikedComments.First(c => c.Id == comment.Id));
                comment.RemoveLike(this);
            }

            DislikedComments!.Add(comment);
            comment.Dislike(this);
        }
    }

    public bool Liked<T>(int interactableId) where T : class
    {
        if (typeof(T) == typeof(Post))
            return LikedPosts!.Any(p => p.Id == interactableId);

        return typeof(T) == typeof(Comment) && LikedComments!.Any(c => c.Id == interactableId);
    }

    public bool Disliked<T>(int interactableId) where T : class
    {
        if (typeof(T) == typeof(Post))
            return DislikedPosts!.Any(p => p.Id == interactableId);

        return typeof(T) == typeof(Comment) && DislikedComments!.Any(c => c.Id == interactableId);
    }
}