    using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public bool isReply { get; set; }
    [Required] public required string Content { get; set; }
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public DateTime CommentDate { get; set; } = DateTime.Now;
    public required int AuthorId { get; set; }
    public User Author { get; set; }
    public int? PostId { get; set; }
    public Post? Post { get; set; }
    public int? ParentId { get; set; }
    public Comment? Parent { get; set; }
    [JsonIgnore] public ICollection<Comment>? Replies { get; set; } = new List<Comment>();
    [JsonIgnore] public ICollection<User> LikedByUsers { get; set; } = new List<User>();
    [JsonIgnore] public ICollection<User> DislikedByUsers { get; set; } = new List<User>();
    
    public void Like(User user)
    {
        LikedByUsers.Add(user);
        Likes++;
    }
    
    public void RemoveLike(User user)
    {
        LikedByUsers.Remove(user);
        Likes--;
    }
    
    public void Dislike(User user)
    {
        DislikedByUsers.Add(user);
        Dislikes++;
    }
    
    public void RemoveDislike(User user)
    {
        DislikedByUsers.Remove(user);
        Dislikes--;
    }
}