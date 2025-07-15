using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    [Required] public required string Title { get; set; }
    public string? Content { get; set; }
    public string ImageUrl { get; set; }
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    [JsonIgnore] public ICollection<Comment> Comments { get; set; }
}