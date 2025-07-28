    using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
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
    [JsonIgnore] public ICollection<Comment>? Replies { get; set; }
}