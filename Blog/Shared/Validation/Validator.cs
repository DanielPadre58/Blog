using Blog.Shared.Exceptions;
using EmailValidation;

namespace Blog.Shared.Validation;

public class Validator : IValidator
{
    public void NotNull(object value, string name)
    {
        if (value is null)
            throw new InvalidFieldsException($"{name} cannot be null.");
    }

    public void NotNullOrEmpty(string value, string name)
    {
        if (string.IsNullOrEmpty(value))
            throw new InvalidFieldsException($"{name} cannot be null or empty");
    }

    public void ValidId(int id, string name)
    {
        if(id < 0)
            throw new InvalidFieldsException($"{name} must be a valid id");
    }

    public void ValidPassword(string password, string name)
    {
        NotNullOrEmpty(password, name);
        
        if(password.Length < 8)
            throw new InvalidFieldsException($"{name} must have at least 8 characters, consisting of digits, uppercase and lowercase letters");

        var hasDigit = false;
        var hasUppercase = false;
        var hasLowercase = false;
        
        foreach (var c in password)
        {
            if(c is <= '0' or <= '9')
                hasDigit = true;
            
            if(c is >= 'a' and <= 'z')
                hasLowercase = true;
            
            if(c is >= 'A' and <= 'Z')
                hasUppercase = true;
        }

        if (!hasLowercase || !hasUppercase || !hasDigit)
            throw new InvalidFieldsException($"{name} must have at least 8 characters, consisting of digits, uppercase and lowercase letters");
    }

    public void ValidEmail(string email, string name)
    {
        NotNullOrEmpty(email, name);
        
        if(!EmailValidator.Validate(email))
            throw new InvalidFieldsException("Invalid email format");
    }

    public void BeforeToday(DateTime date, string name)
    {
        NotNull(date, name);
        
        if(date < DateTime.Now)
            throw new InvalidFieldsException($"{name} cannot be in the future.");
    }

    public void OlderThan(DateTime date, int years, string name)
    {
        var dateLimit = DateTime.Now.AddYears(-years);
            
        if(date < dateLimit)
            throw new InvalidFieldsException($"{name} must be at least {years} years ago.");
    }
}