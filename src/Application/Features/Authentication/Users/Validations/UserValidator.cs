namespace BookPro.Application.Features.Authentication.Users.Validations;

public class UserValidator : BaseValidation<User>, IUserValidator
{
    public ValidationResult ValidateUser(User user)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateEntity(user, "User").Message);

        return result;
    }

}
