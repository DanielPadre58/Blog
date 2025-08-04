using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.Users;
using Blog.Application.External_Services;
using Blog.Application.Services.Authentication;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Redis;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Exceptions;
using Blog.Shared.Security;
using Blog.Shared.Validation;

namespace Blog.Application.Services.Users;

public class UserService(
    IUserRepo repository,
    IPasswordHasher hasher,
    ISmptService smpt,
    IUnvalidatedUsersRepo unvalidatedUsersRepo,
    IAuthenticationService authenticationService,
    IRedisRepo redisRepository,
    IValidator validator) : IUserService
{
    public async Task<string> CreateAsync(UserCreationDto userDto)
    {
        validator.NotNull(userDto, "User information");
        
        await ValidateUsernameUniquenessAsync(userDto.Username);

        await ValidateEmailUniquenessAsync(userDto.Email);

        var user = new User(validator)
        {
            Username = userDto.Username,
            Email = userDto.Email,
            Password = string.Empty
        };
        
        validator.ValidPassword(userDto.Password, "Password");

        user.Password = hasher.HashPassword(user, userDto.Password);

        user.Validate();

        var createdUser = await repository.CreateAsync(user);

        await SendVerificationEmailAsync(createdUser);

        return createdUser.Username;
    }

    public async Task<TokenDto> LoginAsync(LoginDto loginData)
    {
        validator.NotNull(loginData, "Login information");
        
        loginData.Validate();
        
        var token = await authenticationService.AuthenticateAsync(loginData);
        var refreshToken = await authenticationService.GenerateRefreshTokenAsync(loginData.Username);

        return new TokenDto(loginData.Username, token, refreshToken);
    }
    
    public async Task<TokenDto> RefreshAsync(string refreshToken, string username)
    {
        validator.NotNullOrEmpty(refreshToken, "Refresh token");

        var user = await repository.GetByUsernameAsync(username);

        await authenticationService.VerifyRefreshTokenAsync(username, refreshToken);

        var newToken = authenticationService.GenerateJwtToken(user);
        var newRefreshToken = await authenticationService.GenerateRefreshTokenAsync(user.Username);

        await redisRepository.SetRefreshTokenAsync(username, newRefreshToken);
        
        return new TokenDto(username, newToken, newRefreshToken);
    }

    public async Task DeleteAsync(string loggedUsername)
    {
        await repository.DeleteAsync(loggedUsername);

        await redisRepository.RemoveRefreshTokenAsync(loggedUsername);
    }

    public async Task<UserDto> EditAsync(UserUpdateDto updatedUser, string loggedUsername)
    {
        validator.NotNull(updatedUser, "Updated user information");
        
        var user = await repository.GetByUsernameAsync(loggedUsername);
        
        user.ChangeFirstName(updatedUser.FirstName);
        user.ChangeLastName(updatedUser.LastName);
        user.ChangeBirthday(updatedUser.Birthday);

        await repository.SaveAsync();

        return new UserDto(user);
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await repository.GetByUsernameAsync(username.ToLower());

        return new UserDto(user);
    }

    public async Task<TokenDto> VerifyUserAsync(string validationCode)
    {
        var username = await unvalidatedUsersRepo.ValidateUserAsync(validationCode);

        var user = await repository.VerifyUserAsync(username);
        
        var token = authenticationService.GenerateJwtToken(user);
        var refreshTokenoken = await authenticationService.GenerateRefreshTokenAsync(user.Username);

        return new TokenDto(user.Username, token, refreshTokenoken);
    }

    public async Task<bool> LikeAsync(Post post, string username)
    {
        validator.NotNull(post, "Post");

        var user = await repository.GetByUsernameAsync(username);
        
        user.Like(post);
        await repository.SaveAsync();
        
        return user.Liked<Post>(post.Id);
    }
    
    public async Task<bool> LikeAsync(Comment comment, string username)
    {
        validator.NotNull(comment, "Comment");

        var user = await repository.GetByUsernameAsync(username);
        
        user.Like(comment);
        await repository.SaveAsync();
        
        return user.Liked<Comment>(comment.Id);
    }

    public async Task<bool> DislikeAsync(Post post, string username)
    {
        validator.NotNull(post, "Post");

        var user = await repository.GetByUsernameAsync(username);
        
        user.Dislike(post);
        await repository.SaveAsync();
        
        return user.Disliked<Post>(post.Id);
    }
    
    public async Task<bool> DislikeAsync(Comment comment, string username)
    {
        validator.NotNull(comment, "Comment");

        var user = await repository.GetByUsernameAsync(username);
        
        user.Dislike(comment);
        await repository.SaveAsync();
        
        return user.Disliked<Comment>(comment.Id);
    }


    public async Task<bool> UserLikedAsync(Post post, string username)
    {
        validator.NotNull(post, "Post");
        
        var user = await repository.GetByUsernameAsync(username);

        return user.Liked<Post>(post.Id);
    }
    
    public async Task<bool> UserLikedAsync(Comment comment, string username)
    {
        validator.NotNull(comment, "Comment");
        
        var user = await repository.GetByUsernameAsync(username);

        return user.Liked<Comment>(comment.Id);
    }

    public async Task<bool> UserDislikedAsync(Post post, string username)
    {
        validator.NotNull(post, "Post");
        
        var user = await repository.GetByUsernameAsync(username);

        return user.Disliked<Post>(post.Id);
    }
    
    public async Task<bool> UserDislikedAsync(Comment comment, string username)
    {
        validator.NotNull(comment, "Comment");
        
        var user = await repository.GetByUsernameAsync(username);

        return user.Disliked<Comment>(comment.Id);
    }

    private async Task SendVerificationEmailAsync(User user)
    {
        var validationCode = Guid.NewGuid().ToString();
        await unvalidatedUsersRepo.AddValidationCodeAsync(user.Username, validationCode);

        await smpt.SendVerificationCodeAsync(user.Email, validationCode);
    }

    private async Task ValidateUsernameUniquenessAsync(string username)
    {
        if (await repository.UsernameExistsAsync(username))
            throw new DuplicatedUsernameException(username);
    }

    private async Task ValidateEmailUniquenessAsync(string email)
    {
        if (await repository.EmailExistsAsync(email))
            throw new DuplicatedEmailException(email);
    }
}