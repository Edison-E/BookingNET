using Serilog;

namespace BookPro.Application.Features.Account.Validations;

public class LoginValidator : BaseValidation<LoginRequest>, ILoginValidator
{
    public ValidationResult ValidLogin(LoginRequest loginDTO)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateEntity(loginDTO, "Credentials").Message);
        result.Message.AddRange(ValidateParameterRequiredString(loginDTO.Email, "Email").Message);
        result.Message.AddRange(ValidateParameterRequiredString(loginDTO.Password, "Password").Message);
        result.Message.AddRange(ValidatePasswordFormat(loginDTO.Password, "Password").Message);
        result.Message.AddRange(ValidateEmailFormat(loginDTO.Email, "Email").Message);

        return result;
    }

    public ValidationResult ValidationCredentials(LoginRequest login, User user)
    {
        ValidationResult result = new ValidationResult();

        if (!login.Email.Equals(user.Email))
        {
            result.Message.Add("Email is incorrect.");
        }

        if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
        {
            result.Message.Add("Password is incorrect.");
        }

        return result;
    }

    public ValidationResult ValidatePasswordFormat(string password, string name)
    {
        ValidationResult result = new ValidationResult();

        if (password.Length < 7 || password.Length > 12)
        {
            result.Message.Add("Password must be between 9 and 12 characters long.");
        }
        return result;
    }

    public ValidationResult ValidateEmailFormat(string email, string name)
    {
        ValidationResult result = new ValidationResult();

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            result.Message.Add($"{name} is not a valid format.");
        }

        return result;
    }
}
