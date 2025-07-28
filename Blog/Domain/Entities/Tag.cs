using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    [Required] public required string Name { get; set; }
    public int Usages { get; set; } = 1;
    
    public void Use()
    {
        Usages++;
    }
}