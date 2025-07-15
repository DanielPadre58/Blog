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
    public required User Author { get; set; }
    public required Post Post { get; set; }
    public Comment? ReplyingTo { get; set; }
    [JsonIgnore] public ICollection<Comment>? Replies { get; set; }
}