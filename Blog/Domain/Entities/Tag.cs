using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    [Required] public required string Name { get; set; }
    public int Usages { get; set; } = 1;
    [JsonIgnore] public ICollection<Post>? Posts { get; set; } = new List<Post>();
    
    public void Use()
    {
        Usages++;
    }
}