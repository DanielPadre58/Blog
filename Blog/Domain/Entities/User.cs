using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Shared.Exceptions;

namespace Blog.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public bool IsVerified { get; set; } = false;
    [Required] public required string Username { get; set; }
    [Required] public required string Email { get; set; }
    [Required] public required string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore] public ICollection<Post>? Posts { get; set; }
    [JsonIgnore] public ICollection<Comment>? Comments { get; set; }
    [JsonIgnore] public ICollection<Post>? LikedPosts { get; set; }

    public void ChangeUsername(string? username)
    {
        if (username == null)
            return;
        
        ValidateUsername(username);
        Username = username;
    }

    public void ChangeFirstName(string? name)
    {
        if(name == null)
            return;
        
        ValidateName(name);
        FirstName = name;
    }

    public void ChangeLastName(string? name)
    {
        if(name == null)
            return;
        
        ValidateName(name);
        LastName = name;
    }

    public void ChangeBirthday(DateTime? birthday)
    {
        if (!birthday.HasValue)
            return;

        ValidateBirthday(birthday.Value);
        Birthday = birthday;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public void Validate()
    {
        ValidateUsername(Username);
        ValidateEmail(Email);
        ValidatePassword(Password);
        ValidateName(FirstName);
        ValidateName(LastName);

        if (Birthday.HasValue)
            ValidateBirthday(Birthday.Value);
    }

    private void ValidateBirthday(DateTime birthday)
    {
        if (birthday > DateTime.Now)
            throw new InvalidFieldsException("Birthday cannot be in the future.", nameof(birthday));
    }

    private void ValidateName(string name)
    {
        if (name != null && string.IsNullOrWhiteSpace(name))
            throw new InvalidFieldsException("First name cannot be empty.", nameof(name));
    }

    private void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidFieldsException("Password cannot be null or empty.", nameof(password));
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidFieldsException("Email cannot be null or empty.", nameof(email));
    }

    private void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty.", nameof(username));
    }
}