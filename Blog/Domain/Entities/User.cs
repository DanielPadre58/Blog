using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Shared.Exceptions;

namespace Blog.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public bool IsVerified { get; set; } = false;
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
        
        ValidateUsername(username);
        Username = username;
    }

    public void ChangeFirstName(string? name)
    {
        if(name == null)
            return;
        
        ValidateName(name);
        FirstName = name;
    }

    public void ChangeLastName(string? name)
    {
        if(name == null)
            return;
        
        ValidateName(name);
        LastName = name;
    }

    public void ChangeBirthday(DateTime? birthday)
    {
        if (!birthday.HasValue)
            return;

        ValidateBirthday(birthday.Value);
        Birthday = birthday;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public void Validate()
    {
        ValidateUsername(Username);
        ValidateEmail(Email);
        ValidatePassword(Password);
        ValidateName(FirstName);
        ValidateName(LastName);

        if (Birthday.HasValue)
            ValidateBirthday(Birthday.Value);
    }

    private void ValidateBirthday(DateTime birthday)
    {
        if (birthday > DateTime.Now)
            throw new InvalidFieldsException("Birthday cannot be in the future.", nameof(birthday));
    }

    private void ValidateName(string name)
    {
        if (name != null && string.IsNullOrWhiteSpace(name))
            throw new InvalidFieldsException("First name cannot be empty.", nameof(name));
    }

    private void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidFieldsException("Password cannot be null or empty.", nameof(password));
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidFieldsException("Email cannot be null or empty.", nameof(email));
    }

    private void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty.", nameof(username));
    }
    
    public void LikePost(Post post)
    {
        if (LikedPosts.Any(p => p.Id == post.Id))
        {
            LikedPosts.Remove(LikedPosts.First(p => p.Id == post.Id));
            post.RemoveLike();
            return;
        }

        if (DislikedPost(post.Id))
        {
            DislikedPosts.Remove(DislikedPosts.First(p => p.Id == post.Id));
            post.RemoveDislike();
        }

        LikedPosts.Add(post);
        post.Like();
    }
    
    public void LikeComment(Comment comment)
    {
        if (LikedComments.Any(c => c.Id == comment.Id))
        {
            LikedComments.Remove(LikedComments.First(c => c.Id == comment.Id));
            comment.RemoveLike();
            return;
        }

        if (DislikedPost(comment.Id))
        {
            DislikedComments.Remove(DislikedComments.First(c => c.Id == comment.Id));
            comment.RemoveDislike();
        }

        LikedComments.Add(comment); 
        comment.Like();
    }
    
    public void DislikePost(Post post)
    {
        if (DislikedPosts.Any(p => p.Id == post.Id))
        {
            DislikedPosts.Remove(DislikedPosts.First(p => p.Id == post.Id));
            post.RemoveDislike();
            return;
        }

        if (LikedPost(post.Id))
        {
            LikedPosts.Remove(LikedPosts.First(p => p.Id == post.Id));
            post.RemoveLike();
        }
            
        DislikedPosts.Add(post);
        post.Dislike();
    }
    
    public void DislikeComment(Comment comment)
    {
        if (DislikedComments.Any(c => c.Id == comment.Id))
        {
            DislikedComments.Remove(DislikedComments.First(c => c.Id == comment.Id));
            comment.RemoveDislike();
            return;
        }

        if (LikedPost(comment.Id))
        {
            LikedComments.Remove(LikedComments.First(c => c.Id == comment.Id));
            comment.RemoveLike();
        }

        DislikedComments.Add(comment);
        comment.Dislike();
    }
    
    public bool LikedPost(int postId)
    {
        return LikedPosts.Any(p => p.Id == postId);
    }
    
    public bool LikedComment(int commentId)
    {
        return LikedComments.Any(c => c.Id == commentId);
    }
    
    public bool DislikedPost(int postId)
    {
        return DislikedPosts.Any(p => p.Id == postId);
    }
    
    public bool DislikedComment(int commentId)
    {
        return DislikedComments.Any(c => c.Id == commentId);
    }
}