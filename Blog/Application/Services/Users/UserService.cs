using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Application.External_Services;
using Blog.Application.Services.Authentication;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Exceptions;
using Blog.Shared.Security;

namespace Blog.Application.Services.Users;

public class UserService(
    IUserRepo repository, 
    IPasswordHasher hasher, 
    ISmtpService smpt, 
    IUnvalidatedUsersRepo unvalidatedUsersRepo,
    IAuthenticationService authenticationService) : IUserService
{
    public async Task<string> CreateAsync(UserCreationDto userDto)
    {
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
        
        return authenticationService.GenerateJwtTokenAsync(createdUser);
    }

    public async Task<string> LoginAsync(LoginDto loginData)
    {
        var token = await authenticationService.AuthenticateAsync(loginData);
        
        return token;
    }

    public async Task DeleteAsync(string username)
    {
        await repository.DeleteAsync(username);
    }

    public async Task<UserDto> EditAsync(string username, UserUpdateDto updatedUser)
    {
        updatedUser.Validate();

        if (updatedUser.Username != null)
            await ValidateUsernameUniquenessAsync(updatedUser.Username);

        var user = await repository.GetByUsernameAsync(username);

        if(!user.IsVerified)
            throw new UnverifiedUserException();
        
        user.ChangeUsername(updatedUser.Username);
        user.ChangeFirstName(updatedUser.FirstName);
        user.ChangeLastName(updatedUser.LastName);
        user.ChangeBirthday(updatedUser.Birthday);

        repository.SaveAsync();
        
        return new UserDto(user);
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await repository.GetByUsernameAsync(username.ToLower());
        
        if(!user.IsVerified)
            throw new UnverifiedUserException();
        
        return new UserDto(user);
    }

    public async Task<UserDto> VerifyUserAsync(string validationCode)
    {
        var username = unvalidatedUsersRepo.ValidateUserAsync(validationCode).Result;

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