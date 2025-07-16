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
    [JsonIgnore] public ICollection<Post>? Posts { get; set; }
    [JsonIgnore] public ICollection<Comment>? Comments { get; set; }
    [JsonIgnore] public ICollection<Post>? LikedPosts { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new ArgumentException("Username cannot be null or empty.", nameof(Username));
        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(Email));
        if (string.IsNullOrWhiteSpace(Password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(Password));
        if(FirstName != null && string.IsNullOrWhiteSpace(FirstName))
            throw new ArgumentException("First name cannot be empty.", nameof(FirstName));
        if(LastName != null && string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(LastName));
        if (Birthday > DateTime.Now)
            throw new ArgumentException("Birthday cannot be in the future.", nameof(Birthday));
    }
}