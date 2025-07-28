using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.DbContext;

public class BlogContext(DbContextOptions<BlogContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.LikedByUsers)
            .WithMany(u => u.LikedPosts)
            .UsingEntity<Dictionary<string, object>>(
                "PostLikes",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .OnDelete(DeleteBehavior.Cascade));
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.DislikedByUsers)
            .WithMany(u => u.DislikedPosts)
            .UsingEntity<Dictionary<string, object>>(
                "PostDislikes",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}