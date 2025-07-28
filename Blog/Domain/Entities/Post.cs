using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Shared.Exceptions;

namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    [Required] public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public ICollection<Tag>? Tags { get; set; } = new List<Tag>();
    [JsonIgnore] public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    [JsonIgnore] public ICollection<User> LikedByUsers { get; set; } = new List<User>();
    [JsonIgnore] public ICollection<User> DislikedByUsers { get; set; } = new List<User>();

    public void Validate()
    {
        if(string.IsNullOrEmpty(Title))
            throw new InvalidFieldsException("Title cannot be null or empty.");
        if(string.IsNullOrEmpty(Content))
            throw new InvalidFieldsException("Content cannot be null or empty.");
    }

    public void Like()
    {
        Likes++;
    }
    
    public void Dislike()
    {
        Dislikes++;
    }
}