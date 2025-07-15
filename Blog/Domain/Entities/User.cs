using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Domain.Entities;

public class User
{
    public int Id { get; set; }
    [Required] public required string Username { get; set; }
    [Required] public required string Email { get; set; }
    [Required] public required string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthday { get; set; }
    [JsonIgnore] public ICollection<Post> Posts { get; set; }
    [JsonIgnore] public ICollection<Comment> Comments { get; set; }
    [JsonIgnore] public ICollection<Post> LikedPosts { get; set; }
}