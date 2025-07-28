using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Application.External_Services;
using Blog.Application.Services.Authentication;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Redis;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Exceptions;
using Blog.Shared.Security;

namespace Blog.Application.Services.Users;

public class UserService(
    IUserRepo repository,
    IPasswordHasher hasher,
    ISmtpService smpt,
    IUnvalidatedUsersRepo unvalidatedUsersRepo,
    IAuthenticationService authenticationService,
    IRedisRepo redisRepository) : IUserService
{
    public async Task<string> CreateAsync(UserCreationDto userDto)
    {
        if (userDto == null)
            throw new InvalidFieldsException("User data cannot be null.");
        
        await ValidateUsernameUniquenessAsync(userDto.Username);

        await ValidateEmailUniquenessAsync(userDto.Email);

        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            Password = string.Empty
        };

        user.Password = hasher.HashPassword(user, userDto.Password);

        user.Validate();

        var createdUser = await repository.CreateAsync(user);

        await SendVerificationEmailAsync(createdUser);

        return createdUser.Username;
    }

    public async Task<TokenDto> LoginAsync(LoginDto loginData)
    {
        if(loginData == null)
            throw new InvalidFieldsException("Login data cannot be null.");
        
        loginData.Validate();
        
        var token = await authenticationService.AuthenticateAsync(loginData);
        var refreshToken = await authenticationService.GenerateRefreshTokenAsync(loginData.Username);

        return new TokenDto(loginData.Username, token, refreshToken);
    }
    
    public async Task<TokenDto> RefreshAsync(string refreshToken, string username)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new InvalidFieldsException("Refresh token is required.");

        if (string.IsNullOrEmpty(username))
            throw new InvalidFieldsException("Username is required.");

        var user = await repository.GetByUsernameAsync(username);

        await authenticationService.VerifyRefreshTokenAsync(username, refreshToken);

        var newToken = authenticationService.GenerateJwtToken(user);
        var newRefreshToken = await authenticationService.GenerateRefreshTokenAsync(user.Username);

        await redisRepository.SetRefreshTokenAsync(username, newRefreshToken);
        
        return new TokenDto(username, newToken, newRefreshToken);
    }

    public async Task DeleteAsync(string username)
    {
        await repository.DeleteAsync(username);

        await redisRepository.RemoveRefreshTokenAsync(username);
    }

    public async Task<UserDto> EditAsync(string username, UserUpdateDto updatedUser)
    {
        updatedUser.Validate();

        var user = await repository.GetByUsernameAsync(username);
        
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
    
    public async Task<bool> LikePostAsync(Post post, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");
        
        if (post == null)
            throw new InvalidFieldsException("Post cannot be null");

        var user = await repository.GetByUsernameAsync(username);
        
        user.LikePost(post);
        await repository.SaveAsync();
        
        return user.LikedPost(post.Id);
    }

    public async Task<bool> DislikePostAsync(Post post, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");
        
        if (post == null)
            throw new InvalidFieldsException("Post cannot be null");

        var user = await repository.GetByUsernameAsync(username);
        
        user.DislikePost(post);
        await repository.SaveAsync();
        
        return user.DislikedPost(post.Id);
    }

    public async Task<bool> UserLikedAsync(Post post, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");
        
        if (post == null)
            throw new InvalidFieldsException("Post cannot be null");
        
        var user = await repository.GetByUsernameAsync(username);

        return user.LikedPost(post.Id);
    }

    public async Task<bool> UserDislikedAsync(Post post, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidFieldsException("Username cannot be null or empty");
        
        if (post == null)
            throw new InvalidFieldsException("Post cannot be null");
        
        var user = await repository.GetByUsernameAsync(username);

        return user.DislikedPost(post.Id);
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