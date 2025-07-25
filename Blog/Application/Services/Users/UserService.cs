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
    public async Task<TokenDto> CreateAsync(UserCreationDto userDto)
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

        var token = authenticationService.GenerateJwtToken(createdUser);
        var refreshTokenoken = await authenticationService.GenerateRefreshTokenAsync(createdUser.Username);

        return new TokenDto(userDto.Username, token, refreshTokenoken);
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

        if (!user.IsVerified)
            throw new UnverifiedUserException();

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

        if (!user.IsVerified)
            throw new UnverifiedUserException();
        
        user.ChangeFirstName(updatedUser.FirstName);
        user.ChangeLastName(updatedUser.LastName);
        user.ChangeBirthday(updatedUser.Birthday);

        await repository.SaveAsync();

        return new UserDto(user);
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await repository.GetByUsernameAsync(username.ToLower());

        if (!user.IsVerified)
            throw new UnverifiedUserException();

        return new UserDto(user);
    }

    public async Task<UserDto> VerifyUserAsync(string validationCode)
    {
        var username = await unvalidatedUsersRepo.ValidateUserAsync(validationCode);

        var user = await repository.VerifyUserAsync(username);

        return new UserDto(user);
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

    public static bool IsExpiredUser(User user) => (DateTime.UtcNow - user.CreatedAt > TimeSpan.FromDays(7)) && !user.IsVerified;
}