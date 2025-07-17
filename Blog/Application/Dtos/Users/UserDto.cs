using Blog.Application.Dtos.User;

namespace Blog.Application.Dtos.Users;

public record UserDto(int Id, string Username, string? FirstName, string? LastName, DateTime? Birthday)
{
    public UserDto(Domain.Entities.User user) : this(
        user.Id,
        user.Username,
        user.FirstName,
        user.LastName,
        user.Birthday
    ){}
    
    public UserDto(UserUpdateDto user) : this(
        0,
        user.Username,
        user.FirstName,
        user.LastName,
        user.Birthday
    ){}
    
    public void Validate()
    {
        Domain.Entities.User.Validate(this);
    }
    
    public static List<UserDto> ToDtoList(List<Domain.Entities.User> users)
    {
        return users.Select(user => new UserDto(user)).ToList();
    }
}