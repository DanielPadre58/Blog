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
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Parent)
            .WithMany(p => p.Replies)
            .HasForeignKey(p => p.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Comment>()
            .HasMany(c => c.LikedByUsers)
            .WithMany(u => u.LikedComments)
            .UsingEntity<Dictionary<string, object>>(
                "CommentLikes",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Comment>()
                    .WithMany()
                    .HasForeignKey("CommentId")
                    .OnDelete(DeleteBehavior.Cascade));
        
        modelBuilder.Entity<Comment>()
            .HasMany(c => c.DislikedByUsers)
            .WithMany(u => u.DislikedComments)
            .UsingEntity<Dictionary<string, object>>(
                "CommentDislikes",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Comment>()
                    .WithMany()
                    .HasForeignKey("CommentId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}